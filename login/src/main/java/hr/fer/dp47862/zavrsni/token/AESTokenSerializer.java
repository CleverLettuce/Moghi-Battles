package hr.fer.dp47862.zavrsni.token;

import java.security.Key;

import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.utils.CryptoUtils;
import hr.fer.dp47862.zavrsni.utils.HashUtils;

public class AESTokenSerializer implements TokenSerializer {

	private Key key;
	private long durationInMillis;
	
	public AESTokenSerializer(Key key, long durationInMillis){
		this.durationInMillis = durationInMillis;
		this.key = key;
	}
	
	@Override
	public String getToken(User user) {
		String plaintext = buildPlaintext(user, durationInMillis);
		return CryptoUtils.encrypt(plaintext, key);
	}
	
	private String buildPlaintext(User user, long duration){
		StringBuilder builder = new StringBuilder();
		builder.append(user.getId());
		builder.append(AESTokenConstants.DELIMITER);
		builder.append(user.getUsername());
		builder.append(AESTokenConstants.DELIMITER);
		long currentTime = System.currentTimeMillis();
		builder.append(currentTime);
		builder.append(AESTokenConstants.DELIMITER);
		long expirationTime = currentTime + duration;
		builder.append(expirationTime);
		builder.append(AESTokenConstants.DELIMITER);
		builder.append(HashUtils.hashString(Long.toString(currentTime)));
		builder.append(AESTokenConstants.DELIMITER);
		
		return builder.toString();
	}

}
