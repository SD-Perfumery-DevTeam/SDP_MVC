## Important Info
```
please make sure the database being used is up to date(ask peter for the latest version)

do a "git rm -rf --cached ." if there is conflict caused by git ingnore not working as intended
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
4. coding using mockDB changed over to actual DB
5. product views updated in frontend
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
Update 14/09/2021
1. add brand to to catalogue view model
2. dynamic display of product type and gender in product list page
3. removal of redundant html head elements in views
4. styling of individual product display view
5. minor styling improvements
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
Update 20/09/2021
1. create admin links partial view for display in cms pages
2. create an index page for cms controller with common links
3. add 'sdp-style' class for customer facing buttons
4. upgrade bootstrap to 5.1.1, plus now uses libman to manage dependency
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
10. styling improvements to cms pages
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
2. images folder re-organised - sub-folders for distinct database objects
```

```
Update 26/09/2021
1. improvements to page layout for add products and edit products
```

```
Update 27/09/2021
1. improvements to page layout for my account and manage role views
```