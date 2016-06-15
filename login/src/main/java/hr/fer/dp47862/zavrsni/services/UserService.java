package hr.fer.dp47862.zavrsni.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import hr.fer.dp47862.zavrsni.dao.DAO;
import hr.fer.dp47862.zavrsni.models.Participation;
import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.token.ExpiredTokenException;
import hr.fer.dp47862.zavrsni.token.InvalidTokenException;
import hr.fer.dp47862.zavrsni.token.TokenManager;
import hr.fer.dp47862.zavrsni.utils.HashUtils;

@Service
@Transactional
public class UserService {

	@Autowired
	private DAO dao;
	@Autowired
	private TokenManager tokenManager;
	
	public User getUser(int id){
		return dao.getUser(id);
	}
	
	public User getUserByEmail(String email){
		return dao.getUserByEmail(email);
	}
	
	public User getUser(String username){
		return dao.getUser(username);
	}
	
	/**
	 * Gets a user that owns the given token.
	 * @param token user token
	 * @return User from token
	 * @throws ExpiredTokenException if token is expired
	 * @throws InvalidTokenException if token is invalid
	 */
	public User getUserFromToken(String token){
		try{
			return tokenManager.getUser(token);
		} catch (Exception e){
			e.printStackTrace();
			return null;
		}
	}
	
	public String getTokenForUser(User user){
		return tokenManager.getToken(user);
	}

	public boolean usernameExists(String username) {
		return getUser(username) != null;
	}

	public boolean emailExists(String email) {
		return getUserByEmail(email) != null;
	}

	public User registerUser(String username,
			String passwordHash, String email) {
		User user = new User();
		user.setUsername(username);
		user.setEmail(email);
		user.setPasswordHash(passwordHash);
		
		try{
			dao.addUser(user);
		} catch (Exception e){
			e.printStackTrace();
			return null;
		}
		
		return user;
	}
	
	public List<Participation> getParticipations(User user){
		return dao.getParticipations(user);
	}
	
	public List<User> getAllUsers(){
		return dao.getAllUsers();
	}

}
