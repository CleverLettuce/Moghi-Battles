package hr.fer.dp47862.zavrsni.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import hr.fer.dp47862.zavrsni.dao.DAO;
import hr.fer.dp47862.zavrsni.models.Game;
import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.token.TokenManager;

@Service
@Transactional
public class GameService {

	@Autowired
	private DAO dao;

	public int createGame(String mapName){
		return dao.createGame(mapName);
	}
	public void joinGame(User user, int gameId, int teamId){
		Game game = new Game();
		game.setId(gameId);
		dao.joinGame(user, game, teamId);
	}
	
	public void report(User user, Game game, int score, boolean winner){
		dao.report(user, game, score, winner);
	}
}
