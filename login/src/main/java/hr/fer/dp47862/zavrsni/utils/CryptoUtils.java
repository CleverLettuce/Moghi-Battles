package hr.fer.dp47862.zavrsni.utils;

import java.security.InvalidKeyException;
import java.security.Key;
import java.security.NoSuchAlgorithmException;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;

import org.apache.tomcat.util.codec.binary.Base64;

import com.google.common.base.Charsets;

public class CryptoUtils {

	public static String encrypt(String plaintext, Key key){
		Cipher cipher = null;
		String encryptedText = null;
		try {
			cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			cipher.init(Cipher.ENCRYPT_MODE, key);
			encryptedText = new String(
					Base64
					.encodeBase64URLSafe(cipher
							.doFinal(plaintext
									.getBytes(Charsets.UTF_8))),
					Charsets.UTF_8);
		} catch (NoSuchAlgorithmException | NoSuchPaddingException | InvalidKeyException 
				| IllegalBlockSizeException | BadPaddingException e) {
			// e.printStackTrace();
		}
		
		return encryptedText;		
	}
	
	public static String decrypt(String cyphertext, Key key){
		Cipher cipher = null;
		String decryptedText = null;
		try {
			cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			cipher.init(Cipher.DECRYPT_MODE, key);
			decryptedText = new String(cipher.doFinal(Base64.decodeBase64(cyphertext)), Charsets.UTF_8);
		} catch (NoSuchAlgorithmException | NoSuchPaddingException | InvalidKeyException 
				| IllegalBlockSizeException | BadPaddingException e) {
			// e.printStackTrace();
		}
		
		return decryptedText;
	}
}
