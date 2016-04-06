package hr.fer.dp47862.zavrsni.response;

public class GenericResponse<T> implements Response<T> {

	private T data;
	private int status;
	public T getData() {
		return data;
	}
	public void setData(T data) {
		this.data = data;
	}
	public int getStatus() {
		return status;
	}
	public void setStatus(int status) {
		this.status = status;
	}

}
