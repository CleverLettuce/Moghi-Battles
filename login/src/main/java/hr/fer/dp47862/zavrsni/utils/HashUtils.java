package hr.fer.dp47862.zavrsni.utils;

import org.springframework.security.crypto.bcrypt.BCrypt;

public class HashUtils {

	private HashUtils() {}

	public static String hashString(String plainText){
		String hash = BCrypt.hashpw(plainText, BCrypt.gensalt()); 
		return hash;
	}
	
	public static boolean checkHash(String plainText, String hashed){
		return BCrypt.checkpw(plainText, hashed);
	}
	
}
