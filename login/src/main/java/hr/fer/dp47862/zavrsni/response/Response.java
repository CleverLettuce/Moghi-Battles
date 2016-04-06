package hr.fer.dp47862.zavrsni.response;

public interface Response<T>{

	T getData();
	int getStatus();
	void setData(T data);
	void setStatus(int status);
}
