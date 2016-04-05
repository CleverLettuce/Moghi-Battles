package hr.fer.dp47862.zavrsni.token;

import java.security.Key;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import hr.fer.dp47862.zavrsni.models.User;

@Component
public class AESTokenManager implements TokenManager{

	private TokenDeserializer deserializer;
	private TokenSerializer serializer;
	
	@Autowired
	private int duration;
	@Autowired
	private Key key;
	
	public AESTokenManager() {
		deserializer = new AESTokenDeserializer(key);
		serializer = new AESTokenSerializer(key, duration);
	}

	@Override
	public User getUser(String token) {
		return deserializer.getUser(token);
	}

	@Override
	public String getToken(User user) {
		return serializer.getToken(user);
	}

}
