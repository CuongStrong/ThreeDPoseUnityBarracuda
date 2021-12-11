
using UnityEngine;
using System; 
using System.IO; 
using System.Text;
using System.Security.Cryptography; 

//----------------------------------------------------------
// 暗号化 & 複合化
//----------------------------------------------------------
public static class Encryptor{
	private const string m_apiKey	= "gadfadgghopuanvaoafdoiaangNVOSEf";
	private const string m_apiIv	= "adkqfilrogpfsflg";

	private static byte[] m_Key		= System.Text.Encoding.UTF8.GetBytes( m_apiKey );
	private static byte[] m_Iv		= System.Text.Encoding.UTF8.GetBytes( m_apiIv );

	//-------------------------------------------------------------------------------------------------------
	// 暗号化
	//-------------------------------------------------------------------------------------------------------
	public static byte[] Encrypt( byte[] data ){
		MemoryStream memoryStream	= new MemoryStream();

		Rijndael rijndael			= Rijndael.Create();
		rijndael.Mode				= CipherMode.CBC;
		rijndael.Key				= m_Key;
		rijndael.IV					= m_Iv;

		CryptoStream cryptoStream	= new CryptoStream( memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write );
		cryptoStream.Write( data, 0, data.Length );
		cryptoStream.Close();

		memoryStream.Close();

		byte[] encryptBytes			= memoryStream.ToArray();

		return encryptBytes;
	}

	//----------------------------------------------------------
	// string ==> 暗号化 ==> Base64
	//----------------------------------------------------------
	public static string EncryptStringToBase64( string str )
	{ 
		if( str.Length == 0 ){
			return "";
		}

		byte[] strBytes		= System.Text.Encoding.UTF8.GetBytes( str );

		byte[] encryptBytes	= Encrypt( strBytes );

		return Convert.ToBase64String( encryptBytes );
	}



	//-------------------------------------------------------------------------------------------------------
	// 複合化
	//-------------------------------------------------------------------------------------------------------
	public static byte[] Decrypt( byte[] data ){ 
		MemoryStream memoryStream	= new MemoryStream();

		Rijndael rijndael			= Rijndael.Create();
		rijndael.Mode				= CipherMode.CBC;
		rijndael.Key				= m_Key;
		rijndael.IV					= m_Iv;

		CryptoStream cryptoStream	= new CryptoStream( memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write );
		cryptoStream.Write( data, 0, data.Length );
		cryptoStream.Close();

		memoryStream.Close();

		byte[] decryptBytes			= memoryStream.ToArray();

		return decryptBytes;
	}

	//----------------------------------------------------------
	// ( string ==> 暗号化 ==> Base64 )
	// 
	//
	// Base64 ==> 複合化 ==> string
	//----------------------------------------------------------
	public static string DecryptStringToBase64( string base64 ){
		if( base64.Length == 0 ){
			return "";
		}

		byte[] base64Bytes		= Convert.FromBase64String( base64 );

		byte[] decryptBytes	= Decrypt( base64Bytes );

		return System.Text.Encoding.UTF8.GetString( decryptBytes );
	}


	//-------------------------------------------------------------------------------------------------------
	// MD5
	//-------------------------------------------------------------------------------------------------------
	public static string MD5( string str ){
		if( str.Length == 0 ){
			return "";
		}

		string result	= "";
		byte[] strBytes	= System.Text.Encoding.UTF8.GetBytes( str );

		MD5CryptoServiceProvider rds	= new MD5CryptoServiceProvider();
		byte[] rdsBytes					= rds.ComputeHash( strBytes );

		rds.Clear();

		result = BitConverter.ToString( rdsBytes ).ToLower().Replace( "-", "" );

		return result;
	}

	//-------------------------------------------------------------------------------------------------------
	// SHA1
	//-------------------------------------------------------------------------------------------------------
	public static string SHA1( string str ){
		if( str.Length == 0 ){
			return "";
		}

		string result	= "";
		byte[] strBytes	= System.Text.Encoding.UTF8.GetBytes( str );

		SHA1CryptoServiceProvider sha1	= new SHA1CryptoServiceProvider();
		byte[] sha1Bytes				= sha1.ComputeHash( strBytes );

		sha1.Clear();

		result = BitConverter.ToString( sha1Bytes ).ToLower().Replace( "-", "" );

		return result;
	}

	//----------------------------------------------------------

}
//----------------------------------------------------------