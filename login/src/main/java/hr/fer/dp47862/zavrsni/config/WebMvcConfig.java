package hr.fer.dp47862.zavrsni.config;

import java.util.List;

import org.springframework.context.annotation.Configuration;
import org.springframework.http.converter.HttpMessageConverter;
import org.springframework.http.converter.json.MappingJackson2HttpMessageConverter;
import org.springframework.scheduling.concurrent.ThreadPoolTaskExecutor;
import org.springframework.web.servlet.config.annotation.AsyncSupportConfigurer;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurerAdapter;

import com.fasterxml.jackson.annotation.JsonInclude.Include;
import com.fasterxml.jackson.databind.ObjectMapper;

@Configuration
public class WebMvcConfig extends WebMvcConfigurerAdapter {

	private static final long ASYNC_TIMEOUT = 1_000_000_000;
	
    @Override
    public void configureMessageConverters(List<HttpMessageConverter<?>> converters) {
        final MappingJackson2HttpMessageConverter converter = new MappingJackson2HttpMessageConverter();
        final ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.setSerializationInclusion(Include.NON_NULL);
        converter.setObjectMapper(objectMapper);
        converters.add(converter);
        super.configureMessageConverters(converters);
    }
    
    @Override
    public void configureAsyncSupport(AsyncSupportConfigurer configurer) {
    	super.configureAsyncSupport(configurer);
    	configurer.setDefaultTimeout(ASYNC_TIMEOUT);
    	configurer.setTaskExecutor(new ThreadPoolTaskExecutor());
    }

}
