# E621 API C# Wrapper

NuGet package for interacting with the e621 API.

## Features

### Posts

- [x] Create Posts (uploads by URL)
- [x] Vote on Posts
- [x] Favorite Posts
- [x] Flag Posts
- [x] List Posts
- [x] List flag records and flagged posts
- [x] Update Posts

### Tags

- [x] List Tags
- [x] List Tag Aliases

### Notes

- [x] Create Notes
- [x] List Notes
- [x] Delete Notes
- [x] Update Notes
- [x] Revert Notes
- [x] List Note Versions

### Pools

- [x] Create Pool
- [x] List Pools
- [x] Update Pools
- [x] Revert Pools
- [x] List Pool Versions

### Comments

- [x] Create Comments
- [x] List Comments
- [x] Update Comments
- [x] Hide and Unhide Comments
- [x] Delete Comments (administrator only, works in theory)

### Users
- [x] Search Users

### Other implemented functions

- [x] Tag autocomplete
- [x] Popular posts
- [x] Wiki and set searches
- [x] List and create blips
- [x] Authentication check and blacklist updates

See [Documentation.md](Documentation.md) for signatures, return values, and the
current endpoint mappings. Posts now use the V2 JSON schema that will be default by end of year.
