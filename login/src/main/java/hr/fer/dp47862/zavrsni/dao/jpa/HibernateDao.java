package hr.fer.dp47862.zavrsni.dao.jpa;

import java.util.Date;
import java.util.List;
import java.util.Set;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;

import hr.fer.dp47862.zavrsni.dao.DAO;
import hr.fer.dp47862.zavrsni.models.Game;
import hr.fer.dp47862.zavrsni.models.Model;
import hr.fer.dp47862.zavrsni.models.Participation;
import hr.fer.dp47862.zavrsni.models.User;

public class HibernateDao implements DAO{

	@Autowired
	private SessionFactory sessionFactory;
	
	@Override
	public User getUser(String username) {
		return (User)sessionFactory
				.getCurrentSession()
				.getNamedQuery(User.Q_USERNAME)
				.setParameter(User.Q_PARAM_USERNAME, username)
				.uniqueResult();
	}

	@Override
	public User getUser(int id) {
		return (User)sessionFactory.getCurrentSession().byId(User.class).load(id);
	}
	
	@Override
	public User getUserByEmail(String email) {
		return (User)sessionFactory
				.getCurrentSession()
				.getNamedQuery(User.Q_EMAIL)
				.setParameter(User.Q_PARAM_EMAIL, email)
				.uniqueResult();
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<User> getAllUsers() {
		return (List<User>)sessionFactory
				.getCurrentSession()
				.getNamedQuery(User.Q_ALL)
				.list();
	}

	@Override
	public void addUser(User user) {
		persistModel(user);
	}

	@Override
	public void removeUser(int userId) {
		User user = getUser(userId);
		deleteModel(user);
	}

	@Override
	public void updateUser(User user) {
		updateModel(user);
	}
	
	private void deleteModel(Model model){
		Session session = sessionFactory.getCurrentSession();
		session.delete(model);
		session.flush();
	}
	
	private void updateModel(Model model){
		Session session = sessionFactory.getCurrentSession();
		model.setTimeUpdated(new Date());
		session.merge(model);
		session.flush();
	}

	private void persistModel(Model model){
		Session session = sessionFactory.getCurrentSession();
		model.setTimeCreated();
		model.setTimeUpdated(model.getTimeCreated());
		session.persist(model);
		session.flush();
	}
	
	@SuppressWarnings("unused")
	private int saveModel(Model model){
		Session session = sessionFactory.openSession();
		model.setTimeCreated();
		model.setTimeUpdated(model.getTimeCreated());
		int id = (Integer) session.save(model);
		session.flush();
		session.close();
		
		return id;
	}
	
	@Override
	public void joinGame(User user, Game game, int teamId) {
		Participation participation = new Participation();
		participation.setGame(game);
		participation.setUser(user);
		participation.setTeamId(teamId);
		persistModel(participation);
	}

	@Override
	public void report(User user, Game game, int score, boolean winner) {
		Participation part = getParticipation(user, game);
		part.setScore(score);
		part.setWinner(winner);
		updateModel(part);
	}

	@Override
	public Participation getParticipation(int id) {
		return (Participation) sessionFactory
		.getCurrentSession()
		.byId(Participation.class)
		.load(id);
	}

	@Override
	public List<Participation> getParticipations(User user) {
		List<Participation> participations = this.getUser(user.getUsername()).getParticipations();
		if (!participations.isEmpty()){
			// (ノಠ益ಠ)ノ彡┻━┻
			participations.get(0);
		}
		
		return this.getUser(user.getUsername()).getParticipations();
	}

	@Override
	public int createGame(String mapName) {
		Game game = new Game();
		game.setMapName(mapName);
		int gameId = saveModel(game);
		return gameId;
	}

	@Override
	public Participation getParticipation(User user, Game game) {
		for (Participation part : getParticipations(user)){
			if (part.getGame().equals(game)){
				return part;
			}
		}
		return null;
	}

}
