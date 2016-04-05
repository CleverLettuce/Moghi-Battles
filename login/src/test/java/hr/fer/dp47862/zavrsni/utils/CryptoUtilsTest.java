package hr.fer.dp47862.zavrsni.utils;

import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import org.junit.Assert;
import org.junit.Test;
import org.springframework.security.crypto.keygen.KeyGenerators;

import com.google.common.base.Charsets;

public class CryptoUtilsTest {
	
	@Test
	public void testEncDec(){
		String test = new String("test".getBytes(), Charsets.UTF_8);
		SecretKey key = new SecretKeySpec (KeyGenerators.secureRandom(16).generateKey(), "AES");
		String encText = CryptoUtils.encrypt(test, key);
		System.out.println(encText);
		Assert.assertNotEquals(test, encText);
		String decText = CryptoUtils.decrypt(encText, key);
		System.out.println(decText);
		Assert.assertEquals(decText, test);
	}
}
