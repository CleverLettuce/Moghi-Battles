package hr.fer.dp47862.zavrsni.token;

import hr.fer.dp47862.zavrsni.models.User;

public interface TokenDeserializer {

	User getUser(String token);
}
