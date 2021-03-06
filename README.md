## Important Info
```
please make sure the database being used is up to date(ask peter for the latest version)

do a "git rm -rf --cached ." if there is conflict caused by git ingnore not working as intended

library files are now excluded in .gitignore (bootstrap, jquery, etc.).

***Please enable the 'Enable Restore Client Libraries on Build' setting by right
clicking on the libman.json file in the SDPWeb section of the project.***

***If your commits try to uploade these files, please run the "git rm -rf --cached ."
command and try again before committing.***

***Please copy the nlog.config file from the root directory into your debug and release folders***
/SDP/bin/Debug/net5.0/{nlog.config}
/SDP/bin/Release/net5.0/{nlog.config}

```

## Change Overview

```
Update 02/09/2021
1. moved customer abstract class and added customer interface and all the syntax changes that's needed
2. database connection for product and category models
3. adding a parent class to admin and customer that fits the database model
4. name changes on prop to fit the database model
```

```
Update 03/09/2021
1. able adding product too database
2. code clean up
```

```
Update 12/09/2021
1. adding brand model to fit db
2. corresponding cms items
3. MockDb deleted
4. and coding using mockDB changed over to actual DB
```

```
Update 13/09/2021
1. adding img picker
2. img storing to db
3. appsetting.json removed from repo
4. product brand added to display page
5. validation added
```

```
Update 15/09/2021
1. adding inventory
2. adding edit product functionality and page
3. adding delete product functionality and page
4. adding edit inventory functionality and page
5. adding delete inventory functionality and page
```

```
Update 22/09/2021
1. adding User Auth
2. adding email sending service and confirm email function
3. adding roles and restrictions based on said roles
4. adding ability for user to delete their account
5. adding ability for Super Admin to edit delete roles from users
6. adding views for above functions 
7. restricting the product view page to only display 20 items
8. adding multi page display for product view
9. adding the role of customer to all newly registered
```

```
Update 23/09/2021
1. adding reset for forgot password 
2. adding email service
3. deleting code that deals with sending email in account controller
```

```
Update 24/09/2021
1. adding reset for My Account page
```

```
Update 11/10/2021
1. remove external library files from repo and configure libman.json
2. small styling improvements
3. switch to bootstrap icons
```