/**
* Copyright 2019 Google LLC.
* SPDX-License-Identifier: Apache-2.0
*/

function getEnvironment() {
 var environment = {
   generalSheetID: "1w_AtgkwHEchwIis5VwYlj_JcExJfo_gAGBn1mIHXZ4E",
   firebaseUrl: "https://monstermatch3d.firebaseio.com/",
   firestoreUrl: "https://monstermatch3d.firebaseio.com"
 };
 return environment;
}

function onOpen() {
  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var menuEntries = [
    {name: "Export to Dev", functionName: "exportToDev"},
    {name: "Export to Staging", functionName: "exportToStaging"},
    {name: "Export to Production", functionName: "exportToProduction"}
  ];
  ss.addMenu("Export Data", menuEntries);
}

function exportToDev() {
  exportAll('dev');
}

function exportToStaging() {
  exportAll('sta');
}

function exportToProduction() {
  exportAll('pro');
}

function exportAll(envName) {
  var firestore = getFirestoreObject();
  writeDataToFirebase(getEnvironment().generalSheetID, envName, firestore);
}
 

// Creates a Google Sheets on change trigger for the specific sheet
function createSpreadsheetEditTrigger(sheetID) {
 var triggers = ScriptApp.getProjectTriggers();
 var triggerExists = false;
 for (var i = 0; i < triggers.length; i++) {
   if (triggers[i].getTriggerSourceId() == sheetID) {
     triggerExists = true;
     break;
   }
 }

 if (!triggerExists) {
   var spreadsheet = SpreadsheetApp.openById(sheetID);
   ScriptApp.newTrigger("importSheet")
     .forSpreadsheet(spreadsheet)
     .onChange()
     .create();
 }
}

// Delete all the existing triggers for the project
function deleteTriggers() {
 var triggers = ScriptApp.getProjectTriggers();
 for (var i = 0; i < triggers.length; i++) {
   ScriptApp.deleteTrigger(triggers[i]);
 }
}

// Initialize
function syncGeneralSheet (e) {
 writeDataToFirebase(getEnvironment().generalSheetID);
}

// Write the data to the Firebase URL
function writeDataToFirebase(sheetID, envName, firestoreObj) {
 var ss = SpreadsheetApp.openById(sheetID);
 SpreadsheetApp.setActiveSpreadsheet(ss);
 // createSpreadsheetEditTrigger(sheetID);
 var sheets = ss.getSheets();
 for (var i = 0; i < sheets.length; i++) {
   SpreadsheetApp.setActiveSheet(sheets[i]);
   importSheet(envName, firestoreObj);
 }
}

// A utility function to generate nested object when
// given a keys in array format
function assign(obj, keyPath, value) {
 lastKeyIndex = keyPath.length - 1;
 for (var i = 0; i < lastKeyIndex; ++i) {
   key = keyPath[i];
   if (!(key in obj)) obj[key] = {};
   obj = obj[key];
 }
 obj[keyPath[lastKeyIndex]] = value;
}

// Import each sheet when there is a change
function importSheet(envName, firestoreObj) {
  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = SpreadsheetApp.getActiveSheet();
  var name = sheet.getName();
  var data = sheet.getDataRange().getValues();
  var suffix = envName || 'dev';
  
  if (!data[0][0]) {
    return;
  }

  console.log('Exporting: ' + name);
  
  var dataToImport = {};
  
  var isKeyValueOnly = true;
  var valueColumnIndex = 1;
  
  for (var j = 0; j < data[0].length; j++) {
    if (data[1][j] === "ignore")
    {
      continue;
    }
    
    if (data[0][j] === "value") {
      valueColumnIndex = j;
    }
    
    if (data[0][j] !== "id" && data[0][j] !== "value")
    {
      isKeyValueOnly = false;
      break;
    }
  }

  var firestore = firestoreObj || getFirestoreObject();
  
  var i = 1;
  var rowIdx = 0;
  
  var BATCH = 100;
  var N = BATCH;
  while (i < data.length) {
    N = Math.min(N, data.length);
    
    for (var i; i < N; i++) {
      if (!data[i][0]) {
        continue;
      }
      
      if (isKeyValueOnly) {
        if (data[i][0] === "schema") {
          continue;
        }
        dataToImport[data[i][0]] = data[i][valueColumnIndex];
      } else {
        dataToImport[data[i][0]] = {};
        for (var j = 0; j < data[0].length; j++) {
          assign(dataToImport[data[i][0]], data[0][j].split("__"), data[i][j]);
        }
        
        if (data[i][0] === "schema") {
          dataToImport[data[i][0]].rowIdx = "int";
        } else {
          dataToImport[data[i][0]].rowIdx = rowIdx;
          ++rowIdx;
        }
      }
    }
    
    if (Object.keys(dataToImport).length == 0) {
      return;
    }
    
    firestore.updateDocument('data_config' + '_' + suffix + '/' + name, dataToImport);
    N = i + BATCH;
  }
}

function getFirestoreObject () {
  var email = "monstermatch3d@appspot.gserviceaccount.com";
  var key = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDGepYgslup4nCO\nrVHeldWo3kXnvNTSceDIujN/xOIBU1eTTDFtc65U35Y+fQ6N5hPtDDyL5lhrSdxv\ntuLESzo0gMjbccH5WxurpxXF+bvJ1bPYj8D1JtWnHnnJ9R3g/9qWYSBEglKZBZWO\n9CdBGLO8Jp62L0w8tcWWu82IhXfd6yBDtDYZouUHwR9l3otdX/iVKPiFOahVSWEh\nrmzk1vt7OTXAmLno4/Jtg3Q9CUoYYYv9YNqW0WyT6dUH2MOtwdbZgH27vOXkbSEU\nuUFtX1Pnk0Vuz9UE8e/bNAOMabVwQo8Al7wzOK7I1Idy9uJFV8hbVSnwar/SJdmf\n9gBbKOgvAgMBAAECggEACuatuzMAnNaOhw+W6QpCt2kp3r381Ggjx/KOJIzcOuVo\ncHMlXdUWNJwpJNp6g6mo24KqCqcGoTHn1Qzu7IgTNNID9iWL2VW7MdTV+c8rFL1T\niOfwWaad4nVmjASV9Qcoasz/2DNuNPBdFAJt64HfUUyvt+LmJYl4RjyfX8mw2TPZ\ncUSqKtSQMgLOWPNnEJP40lFeKRsdxXRnaiJEh226qaAGljqrLMfJNYYY4LfcfnH6\n2RDo0tijLtDVnE2T+KKcgvB5xDLlcpAPmt99fDh9O6Esq5alxHgiBmmelViBMaLo\ng0sTwXXqozpC3tnveAoU/mvZrPXqReymyc86Y7fLYQKBgQD1/ZatJiKj8EbiVlsB\nHsZSRgtaZzDpxlbBO3YDi+j/zu/JMPFuRBb0yWyOHPHNkNifTbQuctwNvCdRzvHR\nvSDOurCm/QfUX/uvwCYXOldT30HwXMUFYEa5i0Ii0m39KIL0EFOsowZfD1pxujQJ\nr5NZQiNVK+Z+HE37QFVkhDu0jwKBgQDOjhUKZEhQ1vh9BAKPnZRk0lIJDLJON72v\nMAfuVGUjy6xftKdfmMi5afY5z/T6SPxeXq9Y6STprJ+rJZ1sCh66RlsmdYw8DPWS\n1ULzUINBj+GCP8k2VWlawjC/w69eX5EU3HPHjPLCAXViRoTgezMz2rnX1twA3AFI\nD+ZLDMmiYQKBgQDNT4FyNPEWmVXgVSkRj0kBpDqXkqcNhyAjmWhPX3fyAhq6dmIb\noTLzziYPmvLqmuliVK19Uxp9UDwVGL9WSh9WzBTi2y+svwvHTX29R1FZzG4xmoxK\nh2egNy3U+IMal7Rs1i+wWETaFGttnncOWn+GMSBiVwjC2wbvHGvpQEDA2wKBgFrE\nRuy1Gq4jeTh+jZ2MkownfBMr+CK1De6w/Zs2jOdI9itsWGyC2BnaPXA0OSPO9M+H\nbih081qYF+X+donmfJBAoIdq+j/dNgDbJDuFstgZoPA6lXIB4HUYhpGOLT8cYcW4\nBky3XGnyeCHCeVun590ujaISccUS6KFBE2MBcI2hAoGAV71ennqgEL/7wlCqIKOe\nMdJRShRKe93IXeVucrYjBn8hql4swuivSii5gAtMV3lb133Sg+ZbdwDBSS2OOPEO\nS6ysdD1ukKnjhp0YTJrJRkUELb9jTrLkCrpdCMdqXCmTolaASVhCDo0mPKfMiGuo\nWMECli2HW7BHF6G1bAFUyGM=\n-----END PRIVATE KEY-----\n";
  var projectId = "monstermatch3d";
  
  var firestore = FirestoreApp.getFirestore (email, key, projectId);
  return firestore;
}
