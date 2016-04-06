package hr.fer.dp47862.zavrsni.response;

import hr.fer.dp47862.zavrsni.models.User;

public class ResponseUtils {

	private ResponseUtils() {}
	
	public static Response<String> getToken(int status, User user, String token){
		PUNAuthResponse response = new PUNAuthResponse();
		response.setStatus(status);
		if (user != null){
			response.setNickname(user.getUsername());
			response.setUserId(user.getId());
		}
		response.setData(token);
		
		return response;
	}
	
	public static <T> Response<T> getResponse(int status, T data){
		Response<T> response = new GenericResponse<>();
		response.setData(data);
		response.setStatus(status);
		
		return response;
	}
	
	public static User publicUser(User user){
		user = privateUser(user);
		user.setEmail(null);
		user.setTimeUpdated(null);
		
		return user;
	}
	
	public static User privateUser(User user){
		user.setPasswordHash(null);
		return user;
	}
	
	public static Response<User> userResponse(int status, User user, boolean isPublic){
		if (user == null){
			return getResponse(status, null);
		}
		if (isPublic){
			return getResponse(status, publicUser(user));
		} else {
			return getResponse(status, privateUser(user));
		}
	}
}
