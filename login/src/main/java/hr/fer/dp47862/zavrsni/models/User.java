package hr.fer.dp47862.zavrsni.models;

import java.util.HashSet;
import java.util.Set;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinTable;
import javax.persistence.ManyToMany;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.OneToMany;
import javax.persistence.OneToOne;
import javax.persistence.Table;
import javax.persistence.JoinColumn;

@Entity
@Table(name="users")
@NamedQueries({
	@NamedQuery(name=User.Q_USERNAME,query="select user from User as user where user.username=:" + User.Q_PARAM_USERNAME),
	@NamedQuery(name=User.Q_EMAIL,query="select user from User as user where user.username=:" + User.Q_PARAM_EMAIL),
	@NamedQuery(name=User.Q_ALL,query="select user from User as user")
})
public class User extends Model {

	public static final String Q_USERNAME = "User.findByUsername";
	public static final String Q_PARAM_USERNAME = "username_param";
	public static final String Q_EMAIL = "User.findByEmail";
	public static final String Q_PARAM_EMAIL = "email_param";
	public static final String Q_ALL = "User.findAll";
	
	private String firstName;
	private String lastName;
	@Column(nullable = false, unique = true)
	private String username;
	@Column(nullable = false)
	private String passwordHash;
	@Column(nullable = false, unique = true)
	private String email;
	@ManyToMany(fetch=FetchType.LAZY, cascade = {CascadeType.PERSIST})
	@JoinTable(
			   name = "user_blocks", 
			   joinColumns = @JoinColumn(name = "user_id"), 
			   inverseJoinColumns = @JoinColumn(name = "blocked_id")
			 )
	private Set<User> blockedUsers = new HashSet<User>();
	private boolean deactivated = false;
	private int battleCredits;
	
	public String getLastName() {
		return lastName;
	}
	public void setLastName(String lastName) {
		this.lastName = lastName;
	}
	public String getUsername() {
		return username;
	}
	public void setUsername(String username) {
		this.username = username;
	}
	public String getPasswordHash() {
		return passwordHash;
	}
	public void setPasswordHash(String passwordHash) {
		this.passwordHash = passwordHash;
	}
	public String getEmail() {
		return email;
	}
	public void setEmail(String email) {
		this.email = email;
	}
	public String getFirstName() {
		return firstName;
	}
	public void setFirstName(String firstName) {
		this.firstName = firstName;
	}
	public Set<User> getBlockedUsers() {
		return blockedUsers;
	}
	public void setBlockedUsers(Set<User> blockedUsers) {
		this.blockedUsers = blockedUsers;
	}
	public boolean isDeactivated() {
		return deactivated;
	}
	public void setDeactivated(boolean deleted) {
		this.deactivated = deleted;
	}
	public int getBattleCredits() {
		return battleCredits;
	}
	public void setBattleCredits(int battleCredits) {
		this.battleCredits = battleCredits;
	}
	
}
