package hr.fer.dp47862.zavrsni.controllers;

import java.util.ArrayList;
import java.util.List;

import javax.net.ssl.SSLEngineResult.Status;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import hr.fer.dp47862.zavrsni.models.Participation;
import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.response.Response;
import hr.fer.dp47862.zavrsni.response.ResponseUtils;
import hr.fer.dp47862.zavrsni.response.StatusCodes;
import hr.fer.dp47862.zavrsni.services.UserService;
import hr.fer.dp47862.zavrsni.utils.HashUtils;

@RestController
public class UserController {
	
	@Autowired private UserService userService;
	
	public static final int MIN_PASS_LENGTH = 8;
	public static final int MIN_USERNAME_LENGTH = 3;
	
	public class UserScore{
		public String user;
		public int score;
	}
	
	@RequestMapping(value = "/user", method = RequestMethod.POST)
    public Response<User> register(
    		@RequestParam(value="username") String username,
    		@RequestParam(value="password") String password,
    		@RequestParam(value="email") String email
    		) {
		
		if (!checkUsername(username)){
			return ResponseUtils.getResponse(StatusCodes.BAD_USERNAME, null);
		}
		
		if (!checkPassword(password)){
			return ResponseUtils.getResponse(StatusCodes.BAD_PASSWORD, null);
		}
		
		if (!checkEmail(email)){
			return ResponseUtils.getResponse(StatusCodes.BAD_EMAIL, null);
		}
		
		if (userService.usernameExists(username)){
			return ResponseUtils.getResponse(StatusCodes.USERNAME_EXISTS, null);
		}
		
		if (userService.emailExists(email)){
			return ResponseUtils.getResponse(StatusCodes.EMAIL_EXISTS, null);
		}
		
		User user = userService.registerUser(username, HashUtils.hashString(password), email);
		
		return ResponseUtils.userResponse(StatusCodes.OK, user, false);

	}
	
	@RequestMapping(value = "/token-request-pun", method = RequestMethod.GET)
    public Response<String> getToken(
    		@RequestParam(value="username") String username,
    		@RequestParam(value="password") String password
    		) {
		User user = userService.getUser(username);
		
		if (user == null || !HashUtils.checkHash(password, user.getPasswordHash())){
			return ResponseUtils.getToken(StatusCodes.AUTH_FAIL, null, null);
		}
		
		return ResponseUtils.getToken(StatusCodes.OK, ResponseUtils.privateUser(user),
				userService.getTokenForUser(user));
	}
	
	@RequestMapping(value = "/user", method = RequestMethod.GET)
    public Response<User> getUser(
    		@RequestParam(value="username") String username,
    		@RequestParam(value="token") String token
    		) {
		System.out.println(token);
		
		User userSearched = userService.getUser(username);
		User userCurrent = userService.getUserFromToken(token);
		
		if (userSearched == null){
			return ResponseUtils.userResponse(StatusCodes.NOT_FOUND, null, false);
		}
		
		return ResponseUtils.userResponse(StatusCodes.OK, userSearched, !userSearched.equals(userCurrent));
	}
	
	@RequestMapping(value = "/scores", method = RequestMethod.GET)
	public Response<List<UserScore>> userScores(){
		List<User> users = userService.getAllUsers();
		List<UserScore> userScores = new ArrayList<>();
		for (User user : users){
			UserScore userScore = new UserScore();
			userScore.user = user.getUsername();
			userScore.score = 0;
			List<Participation> participations = userService.getParticipations(user);
			participations.forEach((part) -> userScore.score += part.getScore());
			userScores.add(userScore);
		}
		
		return ResponseUtils.getResponse(StatusCodes.OK, userScores);
	}

	private boolean checkEmail(String email) {
		if (!email.contains("@")){
			return false;
		}
		
		return true;
	}

	private boolean checkPassword(String password) {
		if (password.length() < MIN_PASS_LENGTH){
			return false;
		}
		
		return true;
	}

	private boolean checkUsername(String username) {
		if (username.length() < MIN_USERNAME_LENGTH){
			return false;
		}
		if (!username.matches("[a-zA-z0-9]*")){
			return false;
		}
		
		return true;
	}
	
}
