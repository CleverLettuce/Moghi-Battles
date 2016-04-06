package hr.fer.dp47862.zavrsni.response;

public class PUNAuthResponse implements Response<String>{

	private int ResultCode;
	private int UserId;
	private String Nickname;
	private String Data;
	
	@Override
	public String getData() {
		return Data;
	}
	@Override
	public int getStatus() {
		return ResultCode;
	}
	@Override
	public void setData(String token) {
		Data = token;
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
