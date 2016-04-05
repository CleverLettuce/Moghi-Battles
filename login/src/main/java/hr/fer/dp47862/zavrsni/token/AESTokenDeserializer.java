package hr.fer.dp47862.zavrsni.token;

import java.security.Key;
import java.util.NoSuchElementException;
import java.util.Scanner;

import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.utils.CryptoUtils;

public class AESTokenDeserializer implements TokenDeserializer{

	private Key key;
	
	public AESTokenDeserializer(Key key) {
		super();
		this.key = key;
	}

	@Override
	public User getUser(String token) {
		String decrypted = CryptoUtils.decrypt(token, key);
		return parseToken(decrypted);
	}

	private User parseToken(String token) {
		User user = new User();
		try (Scanner sc = new Scanner(token)){
			sc.useDelimiter(";");
			user.setUsername(sc.next());
			long currentTime = System.currentTimeMillis();
			long startTime = Long.parseLong(sc.next());
			long expirationTime = Long.parseLong(sc.next());
			
			long maxDuration = expirationTime - startTime;
			long actualDuration = currentTime - startTime;
			
			if (actualDuration > maxDuration){
				throw new ExpiredTokenException(token, startTime, maxDuration, actualDuration);
			}
		} catch (NoSuchElementException e){
			throw new InvalidTokenException(token, e.getMessage());
		}
		return user;
	}

}
