package hr.fer.dp47862.zavrsni.dao;

import java.util.List;
import java.util.Set;

import hr.fer.dp47862.zavrsni.models.Game;
import hr.fer.dp47862.zavrsni.models.Participation;
import hr.fer.dp47862.zavrsni.models.User;

public interface DAO {

	// user methods
	User getUser(String username);
	User getUserByEmail(String email);
	User getUser(int id);
	List<User> getAllUsers();
	void addUser(User user);
	void removeUser(int userId);
	void updateUser(User user);
	// game methods
	int createGame(String mapName);
	void joinGame(User user, Game game, int teamId);
	void report(User user, Game game, int score, boolean winner);
	Participation getParticipation(int id);
	Participation getParticipation(User user, Game game);
	List<Participation> getParticipations(User user);
	
}
