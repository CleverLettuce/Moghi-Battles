package hr.fer.dp47862.zavrsni.response;

import java.util.HashMap;
import java.util.Map;

public class PUNAuthResponse implements Response<String>{

	private int ResultCode;
	private int UserId;
	private String Nickname;
	private Map<String, Object> Data = new HashMap<>();
	
	@Override
	public String getData() {
		return (String) Data.get("token");
	}
	@Override
	public int getStatus() {
		return ResultCode;
	}
	@Override
	public void setData(String token) {
		Data.put("token", token);
	}
	
	@Override
	public void setStatus(int status) {
		switch (status){
		case StatusCodes.OK:
			ResultCode = 1;
			break;
		default:
			ResultCode = 2;
		}
	}
	public int getUserId() {
		return UserId;
	}
	public void setUserId(int userId) {
		UserId = userId;
	}
	public String getNickname() {
		return Nickname;
	}
	public void setNickname(String nickname) {
		Nickname = nickname;
	}
}
