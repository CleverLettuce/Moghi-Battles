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
	Set<User> getBlockedUsers(int id);
	void addUser(User user);
	void removeUser(int userId);
	void updateUser(User user);
	void blockUser(int blockerId, int blockedId);
	void unblockUser(int blockerId, int blockedId);
	void deactivateUser(int userId);
	void activateUser(int userId);
	
}
