# Demo nginx

## Online tool to create nginx config
https://nginxconfig.io/


## Load balancing documentation
http://nginx.org/en/docs/http/load_balancing.html



## Add basic auth in a location

### In host machine:
```bash
sudo apt-get install apache2-utils
```

```bash
htpasswd -c ./src/nginx/.htpasswd <user>
htpasswd -c ./src/nginx/.htpasswd username
```

```nginx
location / {
    auth_basic "Restricted";
    auth_basic_user_file /etc/nginx/.htpasswd;
}
```

### Test

Should return 401
```bash
curl -i http://localhost:9003/contacts
```

Success
```bash
curl -i http://username:password@localhost:9003/contacts
```