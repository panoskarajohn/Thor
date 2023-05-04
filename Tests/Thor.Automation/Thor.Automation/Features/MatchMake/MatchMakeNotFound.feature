Feature: MatchMade not found when only one player on queue

@matchMake @notfound @error
Scenario: Match not found
	Given a player
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	
@matchMake @found
Scenario: Match found
	Given a player
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	Given a player
	When the player searches for a match on the queue
	Then the player should receive a 200 status code
