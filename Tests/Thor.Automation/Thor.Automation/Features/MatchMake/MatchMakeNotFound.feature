Feature: MatchMade not found when only one player on queue
	
Background: 
	Given a clean queue

@matchMake @notfound @error
Scenario: Match not found
	Given a player with elo 1200
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	
@matchMake @found
Scenario: Match found
	Given a player with elo 1200
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	Given a player with elo 1200
	When the player searches for a match on the queue
	Then the player should receive a 200 status code
	And queue should be empty
	
@matchMake @notfound 
Scenario: Match not found for elo higher than 200
	Given a player with elo 1200
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	Given a player with elo 1500
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	And queue should not be empty
