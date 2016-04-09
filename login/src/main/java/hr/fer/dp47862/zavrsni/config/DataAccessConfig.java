package hr.fer.dp47862.zavrsni.config;

import javax.sql.DataSource;

import org.apache.tomcat.dbcp.dbcp.BasicDataSource;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;
import org.springframework.core.env.Environment;
import org.springframework.transaction.annotation.EnableTransactionManagement;

import hr.fer.dp47862.zavrsni.dao.DAO;
import hr.fer.dp47862.zavrsni.dao.jpa.HibernateDao;

import org.springframework.orm.hibernate4.HibernateTransactionManager;
import org.springframework.orm.hibernate4.LocalSessionFactoryBuilder;

@Configuration
@ComponentScan("hr.fer.dp47862.zavrsni")
@PropertySource("classpath:data-access.properties")
@EnableTransactionManagement
public class DataAccessConfig {

	private static final String P_JDBC_DRIVER = "jdbc.driver";
	private static final String P_JDBC_URL = "jdbc.url";
	private static final String P_JDBC_USER = "jdbc.user";
	private static final String P_JDBC_PASS = "jdbc.password";
	
	private static final String P_HIB_AUTO = "hibernate.hbm2ddl.auto";
	private static final String P_HIB_DIALECT = "hibernate.dialect";
	private static final String P_HIB_SHOW_SQL = "hibernate.show_sql";
	private static final String P_HIB_CONN_CS = "hibernate.connection.charSet";
	private static final String P_HIB_CONN_CP = "hibernate.connection.characterEncoding";
	private static final String P_HIB_CONN_USE_UNICODE = "hibernate.connection.useUnicode";
	
	@Autowired
	private Environment env;
	
	@Bean(name = "dataSource")
	public DataSource getDataSource() {
	    BasicDataSource dataSource = new BasicDataSource();
	    dataSource.setDriverClassName(env.getProperty(P_JDBC_DRIVER));
	    dataSource.setUrl(env.getProperty(P_JDBC_URL));
	    dataSource.setUsername(env.getProperty(P_JDBC_USER));
	    dataSource.setPassword(env.getProperty(P_JDBC_PASS));
	 
	    return dataSource;
	}
	
	@Autowired
	@Bean(name = "sessionFactory")
	public SessionFactory getSessionFactory(DataSource dataSource) {
	 
	    LocalSessionFactoryBuilder sessionBuilder = new LocalSessionFactoryBuilder(dataSource);
	    sessionBuilder.scanPackages("hr.fer.dp47862.zavrsni.models");
	    sessionBuilder.setProperty("hibernate.hbm2ddl.auto", env.getProperty(P_HIB_AUTO));
        sessionBuilder.setProperty("hibernate.dialect", env.getProperty(P_HIB_DIALECT));
        sessionBuilder.setProperty("hibernate.show_sql", env.getProperty(P_HIB_SHOW_SQL));
        sessionBuilder.setProperty("hibernate.connection.characterEncoding", env.getProperty(P_HIB_CONN_CP));
        sessionBuilder.setProperty("hibernate.connection.useUnicode", env.getProperty(P_HIB_CONN_USE_UNICODE));
        sessionBuilder.setProperty("hibernate.connection.charSet", env.getProperty(P_HIB_CONN_CS));
        
	    return sessionBuilder.buildSessionFactory();
	}
	
	@Autowired
	@Bean(name = "transactionManager")
	public HibernateTransactionManager getTransactionManager(SessionFactory sessionFactory) {
	    return new HibernateTransactionManager(sessionFactory);
	}
	
	@Bean(name = "dao")
	public DAO getDao() {
	    return new HibernateDao();
	}
}
