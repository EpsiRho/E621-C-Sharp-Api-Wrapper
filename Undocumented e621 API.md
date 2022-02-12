# https://e621.net/comments.json

## Get Queries

* `limit`: The amount of comments to return (Max Unknown)
* **Required** `group_by`: Must be set to "comment". Returns posts instead?
* `format`: The format(html/json) to return in.
* `search[post_id]`: The id to get comments from.
* `search[body_matches]`: Searches all comments for matching strings. Use "*" for wildcard search.
* `search[creator_name]`: Returns comments made by x user.
* `search[creator_id]`: Returns comments made by x user id.

## Response

[
  {
    "id": <int>,
    "created_at": <Date Time>,
    "post_id": <int>,
    "creator_id": <int>,
    "body": <string>
    "score": <int>,
    "updated_at": <Date Time>,
    "updater_id": <int>,
    "do_not_bump_post": <bool>,
    "is_hidden": <bool>,
    "is_sticky": <bool>,
    "creator_name": <string>,
    "updater_name": <string>
  }

]