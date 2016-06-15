package hr.fer.dp47862.zavrsni.config;

import java.security.Key;

import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;
import org.springframework.core.env.Environment;
import org.springframework.security.crypto.keygen.KeyGenerators;

import hr.fer.dp47862.zavrsni.token.AESTokenManager;
import hr.fer.dp47862.zavrsni.token.TokenManager;

@Configuration
@ComponentScan("hr.fer.dp47862.zavrsni")
@PropertySource("classpath:token.properties")
public class TokenConfig {
	
	@Autowired
	private Environment env;
	
	private static final String P_DURATION = "duration";
	private static final int DEFAULT_DURATION = 86400000;
	
	@Bean(name = "duration")
	public int getDuration(){
		String durationProperty = env.getProperty(P_DURATION);
		int duration;
		try{
			duration = Integer.parseInt(durationProperty);
		} catch (Exception e){
			e.printStackTrace();
			duration = DEFAULT_DURATION;
		}
		
		return duration;
	}
	
	@Bean(name = "key")
	public Key getKey(){
		SecretKey key = new SecretKeySpec (KeyGenerators.secureRandom(16).generateKey(), "AES");
		return key;
	}
	
	@Autowired
	@Bean(name = "tokenManager")
	public TokenManager getTokenManager(Key key, int duration) {
	    return new AESTokenManager(key, duration);
	}

}
