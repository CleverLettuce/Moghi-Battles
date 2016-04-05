package hr.fer.dp47862.zavrsni.token;

import hr.fer.dp47862.zavrsni.models.User;

public interface TokenManager {

	User getUser(String token);
	String getToken(User user);
}
