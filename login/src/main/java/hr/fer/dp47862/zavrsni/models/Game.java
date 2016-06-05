package hr.fer.dp47862.zavrsni.models;

import java.util.ArrayList;
import java.util.List;

import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.OneToMany;
import javax.persistence.Table;

@Entity
@Table(name="games")
public class Game extends Model{

	@OneToMany(fetch = FetchType.LAZY, mappedBy = "game")
	private List<Participation> participations = new ArrayList<>();
	private String mapName;
	
	public List<Participation> getParticipations() {
		return participations;
	}
	public void setParticipations(List<Participation> participations) {
		this.participations = participations;
	}
	public String getMapName() {
		return mapName;
	}
	public void setMapName(String mapName) {
		this.mapName = mapName;
	}
	
	
}
