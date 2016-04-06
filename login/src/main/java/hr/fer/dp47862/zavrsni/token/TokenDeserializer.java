package hr.fer.dp47862.zavrsni.token;

import hr.fer.dp47862.zavrsni.models.User;

public interface TokenDeserializer {

	/**
	 * Gets a user that owns the given token.
	 * @param token user token
	 * @return User from token
	 * @throws ExpiredTokenException if token is expired
	 * @throws InvalidTokenException if token is invalid
	 */
	User getUser(String token);
}
