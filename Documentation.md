# Classes

# RequestHost

This is the main class you need. Make sure before you use any function to define the UserAgent, as it is required to use the e621 API. Proper user agent form is `"<Application Name>/<Version> (Your Name)"`.

# RequestHost: Variables

* `UserAgent`: Holds the user agent for the REST request. **Required**

* `Username`: Holds a username for logging in, must also set the ApiKey .

* `ApiKey`: Holds an ApiKey for logging in, must also set the Username.

* `LastError`: Holds an *ErrorDetails* class that gives the details of any errors generated by a function.

# RequestHost: Functions

## GetPosts(string Tags, int Limit , int Page)

Gets a list of posts.

* `Tags`: The tags to look for when getting posts.

* `Limit`: The amount of posts to get. Must be within 1 - 320. (Defaults to 75)

* `Page`: The page of posts to grab. (Defaults to 1)

*Returns*: A list of the Post class

## GetPosts(string Tags, char PaginationChar, int PostId, int Limit)

Gets a list of posts.

* `Tags`: The tags to look for when getting posts.

* `Limit`: The amount of posts to get. Must be within 1 - 320. (Defaults to 75)

* `PaginationChar`: Must be either 'b' for before or 'a' for after.

* `PostId`: The post Id to start at.

*Returns*: A list of the Post class

## VotePost(int PostId, int Vote)

Votes on a post

* `PostId`: The id of the post to vote on.

* `Vote`: The int to detonated if it should up vote or down vote. 1 for Up, -1 for Down.

*Returns*: The response from the server in the vote class. Contains success, total score, and the user's score.

## SearchTags(string Search, TagCategory Category, SortOrder Sort, bool HideEmpty, int Limit, int Page)

Search for tags in the database.

* `Search`: The tag to search for. End with '*' to search for entries starting with.

* `Category`: The category of tag to search under.

* `Sort`: The order to return results in.

* `HideEmpty`: Hide tags with no posts.

* `Limit`: The max number of results to return.

* `Page`: The page of results to show.

*Returns*: A list of a class 'Tag' that holds info on each resulting tag

## SearchTags(string Search, char PaginationChar, int PostId, TagCategory Category = TagCategory.All, SortOrder Sort = SortOrder.date, bool HideEmpty = false, int Limit = 75)

Search for tags in the database.

* `Search`: The tag to search for. End with '*' to search for entries starting with.

* `PaginationChar`: Either 'b' or 'a' (before or after the post id).

* `PostId`: The post Id to start at.

* `Category`: The category of tag to search under.

* `Sort`: The order to return results in.

* `HideEmpty`: Hide tags with no posts.

* `Limit`: The max number of results to return.

*Returns*: A list of a class 'Tag' that holds info on each resulting tag.

## GetPostComments(int PostId, int Limit)

Get comments from a post

* `PostId`: The id of the post.
* `Limit`: The max number of comments to get.

*Returns*: A list of Comment classes

## GetUserComments(int CreatorId, int Limit)

Get comments from a post

* `CreatorId`: The id of the user.
* `Limit`: The max number of comments to get.

*Returns*: A list of Comment classes

## GetUserComments(string CreatorName, int Limit)

Get comments from a post

* `CreatorName`: The id of the user.
* `Limit`: The max number of comments to get.

*Returns*: A list of Comment classes

## SearchComments(string Body, int Limit)

Search comments for a string

* `Body`: A string to search comment bodies for. Use "*" for wildcard search.
* `Limit`: The max number of comments to get.

# ErrorDetails

Holds info about the last error.

* `ResponseCode`: Holds a StatusCode enum value

* `Message`: The status message from the error

# Comment

Holds info on a comment

# Post

A class that holds details on a post. Contains multiple sub-classes.

## Post sub-classes

* Relationships

* Flags
* Tags
* Score
* Sample
* Alternates **(Note: I have never seen this be used at the class is there to prevent json deserialization errors)**
* Preview
* File



