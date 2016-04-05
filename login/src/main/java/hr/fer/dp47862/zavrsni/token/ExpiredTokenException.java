package hr.fer.dp47862.zavrsni.token;

public class ExpiredTokenException extends RuntimeException {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	private String token;
	private long startTime;
	private long maxDuration;
	private long actualDuration;
	
	public ExpiredTokenException() {
	}

	public ExpiredTokenException(String message) {
		super(message);
	}

	public ExpiredTokenException(Throwable cause) {
		super(cause);
	}

	public ExpiredTokenException(String message, Throwable cause) {
		super(message, cause);
	}

	public ExpiredTokenException(String message, Throwable cause, boolean enableSuppression,
			boolean writableStackTrace) {
		super(message, cause, enableSuppression, writableStackTrace);
	}

	public ExpiredTokenException(String token, long startTime, long maxDuration, long actualDuration) {
		super();
		this.token = token;
		this.startTime = startTime;
		this.maxDuration = maxDuration;
		this.actualDuration = actualDuration;
	}

	public String getToken() {
		return token;
	}

	public long getStartTime() {
		return startTime;
	}

	public long getMaxDuration() {
		return maxDuration;
	}

	public long getActualDuration() {
		return actualDuration;
	}
	
	

}
