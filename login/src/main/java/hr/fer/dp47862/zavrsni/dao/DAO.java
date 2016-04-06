package hr.fer.dp47862.zavrsni.dao;

import java.util.List;
import java.util.Set;

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
	
}
