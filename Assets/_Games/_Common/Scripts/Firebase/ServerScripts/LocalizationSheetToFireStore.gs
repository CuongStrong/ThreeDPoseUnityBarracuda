/**
* Copyright 2019 Google LLC.
* SPDX-License-Identifier: Apache-2.0
*/

var cachedDocRef = {};

function getEnvironment() {
 var environment = {
   spreadsheetID: "1cNry-6S75-9yIMNY_pO4gBXXnZIl_k8NFYS_PjE0RyA",
   firebaseUrl: "https://battle-waifus.firebaseio.com/",
   firestoreUrl: "https://battle-waifus.firebaseio.com"
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

function exportToDev () {
  writeDataToFirebase(getEnvironment().spreadsheetID, 'dev');
}

function exportToStaging () {
  writeDataToFirebase(getEnvironment().spreadsheetID, 'sta');
}

function exportToProduction () {
  writeDataToFirebase(getEnvironment().spreadsheetID, 'pro');
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
function initialize(e) {
 writeDataToFirebase(getEnvironment().spreadsheetID, 'dev');
}

// Write the data to the Firebase URL
function writeDataToFirebase(sheetID, envName) {
 var ss = SpreadsheetApp.openById(sheetID);
 SpreadsheetApp.setActiveSpreadsheet(ss);
 // createSpreadsheetEditTrigger(sheetID);
 var sheets = ss.getSheets();
 for (var i = 0; i < sheets.length; i++) {      
   SpreadsheetApp.setActiveSheet(sheets[i]);
   importSheet(envName);   
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
function importSheet(envName) {  
  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = SpreadsheetApp.getActiveSheet();  
  var name = sheet.getName();
  var data = sheet.getDataRange().getValues();
  var suffix = envName || 'dev';
  
  if (!data[0][0]) {
    return;
  }
  
  var prefix = "";
  if (name !== "general") {
    prefix = name + ".";
  }
  
  var languages = [];
  
  var dataToImport = {};
  
  for (var i = 1; i < data[0].length; i++) {
    var langKey = data[0][i];
    if (langKey) {
      languages.push(langKey);
      dataToImport[langKey] = {};
    }
  }   
  
  for (var i = 2; i < data.length; i++) {
    var id = data[i][0];
    if (!id) {
      continue;
    }        
    
    for (var j = 1; j < data[0].length; j++) {      
      var langKey = data[0][j];
      if (langKey) {
        dataToImport[langKey][prefix + id] = data[i][j];
      }      
    }
  }

  if (Object.keys(dataToImport).length == 0) {
    return;
  }

  var email = "battle-waifus@appspot.gserviceaccount.com";
  var key = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDD2ywk9P5b1+4D\nz1I7npEmrCFBjIxZDonVxlW9rvKCbWG8fJ2PzmHjRBH3uT5lz6rAVrt0p9YTStuY\n/ymjAUNWAN94QDV08osuoc5MPv0UpxF63DhxNzlSW8EYvfOp7a8wdhHKV4TzaRQI\nYlp9see/fTBME+5vFhnLdwSB5xCzOrip8F7yAVLepSNqffTeyXHe3Sg4F+d4AClf\naZ+LSlqUieNyKpTih5UwC0vUyp7bqL794TTb4Ww0lOD7BOVs7lI2cT/ubTwu+cHv\nJKwnim1PKbvbYzkYYRK0sIrodUJHHHTs3/C+uwalYQjM/wedLsQRWqYDYYcqaTVw\n6q2HJ+ZRAgMBAAECggEAAqVz973dr/T/cMq81DRkxq59toqFHPDu2NYYs3rpKOz8\nbJUMdfR2ayEw0vWMS8jeCEJDvyRDZrhUtkEAWYT7WW6illfbwQxD6CnkhTA2sDA/\nsrbGBnWWDzBRMBnk8G9XNiLqQ58iGz3YuPJHQbqFJJe27tg4GQUl7RNbGrw6BAnp\ngKSWcUe56O5CDZKrgwIvm+6gEnBnrNWTmqIBSvstO2Adl+y1hwmizLSNBKbxtTL9\ngs0r6JMlaTuBuWHptTenY2ppUopUd9dFCd20GrPm6M4LmxNjzC4NGgCG2i+JUe9H\nYvYd91t9QuPftZ9nlXV1/bgzJn+Of6p8cGJRT+pUXwKBgQD0wiGi6AJnH0bHj4/M\njU2kjMxuDlYFiySHBF9+S6YWzx45Wjru05CHBd4pjFw/qd3lPUAYK7/cat5ZpRq2\ncZ+yWNLGMHYbIKKr4LCo+eYF6SE2v6lbBXEixyFSqzH85DpCd4AHixQ3ADW4ZBSk\n1RDPcF4jwsJedvdDhB8bjVFlYwKBgQDM2gyT5F0c/n0nJJY+9hH05LeJuEqYR5Ay\nRJpEz+mWcHf1057bwphG3+uWcX6aIUGcYOI+cRdNeV3JbW/d/tD6n6odiEr3YDte\nAPCfwRUfEC7aEcDXsY20pYkwzkebHQlsEJ8Zx0BKQmbXIehybsWBOZO/MhtUSoFh\ns1WPToD9uwKBgEp0Q4cqzJBSE1XhdfzjfxrvOcDA26oUCaiIzAO/P2bw2mDX8dV0\nxiJMxqgc6nLzWWzClRzFMjf6ymi6sMWPCuVV38gqNcNKyjE+yH6ehjqGEHR3GS13\nGlNZ/+kF6s7jHlXZxDiHNj8E8VKGH0GFDvgLVpfKRxY1WkJPjqby9i/BAoGAImrk\nQBjXOB7d4iy4fZ2USMQAvkfTaKBlbryt+q+//EGrNgc3Io8gTJAkoeMxtS2vO1nf\nyUPw+VhR1Jzpda/xZppa15lllwh5EQMxanXjWZO7e3IOJ28ycs55LVULHNmVEq38\nPTWH7LIZWz3cMxW/Y0XMOAxwIQpfkI2z617yNy8CgYEAg6pBUlSM0ARj6VGSLpEl\nZpMhgIl4QMw3CyHm1AxodDrYQBwdnXnDuOGfL6lms24jspdtrUWWlWzRM5jFyC7O\nkitfkezFz4dJFN9UhCU6d+9oaNcg3aey0jto5s4FfNP2rs08P4LoxxlYGLQD7rkA\ncFM4qY2vRCodFefUJwdKPH8=\n-----END PRIVATE KEY-----\n";
  var projectId = "battle-waifus";
  
  var firestore = FirestoreApp.getFirestore (email, key, projectId);  
  
  for (var i = 0; i < languages.length; ++i) {
    var langKey = languages[i];
    var docPath = 'localization' + '_' + suffix + '/' + langKey;
    
    var docRef = cachedDocRef[docPath];
    if (!docRef) {
      try {
        docRef = firestore.getDocument(docPath);    
      } catch (ex) {
      }
      if (!docRef) {
        cachedDocRef[docPath] = docRef;
      }
    }
    var oldTexts = docRef ? docRef.obj : {};
    
    var textMap = dataToImport[langKey];    
    var textKeys = Object.keys(textMap);
    
    var batch = {};
    var batchCount = 0;
    for (var j = 0; j < textKeys.length; ++j) {
      var k = textKeys[j];
      var val = textMap[k];
      
      if (val != oldTexts[k]) {      
        batch[k] = val;      
        ++batchCount;
      }
      
      if (batchCount > 20) {
        firestore.updateDocument(docPath, batch, true);
        batch = {};
        batchCount = 0;
      }      
    }       
    
    if (batchCount > 0) {
      firestore.updateDocument(docPath, batch, true);
      batch = {};
      batchCount = 0;
    }      
  }
}