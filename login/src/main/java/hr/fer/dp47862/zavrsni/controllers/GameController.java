package hr.fer.dp47862.zavrsni.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import hr.fer.dp47862.zavrsni.models.Game;
import hr.fer.dp47862.zavrsni.models.User;
import hr.fer.dp47862.zavrsni.response.GenericResponse;
import hr.fer.dp47862.zavrsni.response.Response;
import hr.fer.dp47862.zavrsni.response.ResponseUtils;
import hr.fer.dp47862.zavrsni.response.StatusCodes;
import hr.fer.dp47862.zavrsni.services.GameService;
import hr.fer.dp47862.zavrsni.services.UserService;

@RestController
public class GameController {


	@Autowired 
	private UserService userService;
	
	@Autowired 
	private GameService gameService;
	
	@RequestMapping(value = "/join", method = RequestMethod.POST)
    public Response<Object> joinGame(
    		@RequestParam(value="token") String token,
    		@RequestParam(value="gameId") int gameId,
    		@RequestParam(value="teamId") int teamId
    		) {
		
		User user = userService.getUserFromToken(token);
		
		if (user == null){
			return ResponseUtils.getResponse(StatusCodes.FORBIDDEN, null);
		}
		
		gameService.joinGame(user, gameId, teamId);
		
		return ResponseUtils.getResponse(StatusCodes.OK, null);
	}
	
	@RequestMapping(value = "/create", method = RequestMethod.POST)
    public Response<Object> createGame(
    		@RequestParam(value="token") String token,
    		@RequestParam(value="mapName") String mapName
    		) {
		
		User user = userService.getUserFromToken(token);
		
		if (user == null){
			return ResponseUtils.getResponse(StatusCodes.FORBIDDEN, null);
		}
		
		int gameId = gameService.createGame(mapName);
		
		return ResponseUtils.getResponse(StatusCodes.OK, gameId);
	}
	
	@RequestMapping(value = "/report", method = RequestMethod.POST)
    public Response<Object> report(
    		@RequestParam(value="token") String token,
    		@RequestParam(value="gameId") int gameId,
    		@RequestParam(value="score") int score,
    		@RequestParam(value="winner") boolean winner
    		) {
		
		User user = userService.getUserFromToken(token);
		
		if (user == null){
			return ResponseUtils.getResponse(StatusCodes.FORBIDDEN, null);
		}
		Game game = new Game();
		game.setId(gameId);
		gameService.report(user, game, score, winner);
		
		return ResponseUtils.getResponse(StatusCodes.OK, null);
	}
}
