package networking.request;

// Java Imports
import java.io.IOException;
import java.util.List;

// Other Imports
import core.GameClient;
import core.GameServer;
//import dataAccessLayer.UsersDAO;
import metadata.Constants;
import model.Player;
import networking.response.ResponseLogin;
import utility.DataReader;
import utility.GamePacket;
import utility.Log;

/**
 * The RequestLogin class authenticates the user information to log in. Other
 * tasks as part of the login process lies here as well.
 */
public class RequestLogin extends GameRequest {

  // Data
  private String version;
  private int user_id;
  private String password;
  // Responses
  private ResponseLogin responseLogin;
  private static int count = 0;

  public RequestLogin() {
    responses.add(responseLogin = new ResponseLogin());
  }

  @Override
  public void parse() throws IOException {
    version = DataReader.readString(dataInput).trim();
    //user_id = DataReader.readInt(dataInput);
//        password = DataReader.readString(dataInput).trim();
  }

  @Override
  public void doBusiness() throws Exception {
    Player player = new Player();
    Log.printf("User '%s' is connecting...", player.getID());

    client.setUserID(player.getID());
    responseLogin.setUserID(player.getID());

    GameServer.getInstance().setActivePlayer(player);
    player.setClient(client);
    // Pass Player reference into thread
    client.setPlayer(player);
    // Set response information
    responseLogin.setStatus((short) 0); // Login is a success
    responseLogin.setPlayer(player);
    ++count;



//  //       Checks if the connecting client meets the minimum version required
//        if (version.compareTo(Constants.CLIENT_VERSION) >= 0) {
//            if (!user_id.isEmpty()) {
//                // Verification Needed
//                player = UsersDAO.getUserFromDbIfCredentialsAreValid(user_id, password);
//            }
//            if (player == null) {
//                responseLogin.setStatus((short) 1); // User info is incorrect
//                Log.printf("User '%s' has failed to log in.", user_id);
//            } else {
//                player.setClient(client);
//                if (client.getPlayer() == null || player.getID() != client.getPlayer().getID()) {
//                    GameClient thread = GameServer.getInstance().getThreadByPlayerID(player.getID());
//                    // If account is already in use, remove and disconnect the client
//                    if (thread != null) {
//                        responseLogin.setStatus((short) 2); // Account is in use
//                        thread.removePlayerData();
//                        thread.newSession();
//                        Log.printf("User '%s' account is already in use.", user_id);
//                    } else {
//                        // Continue with the login process
//                        GameServer.getInstance().setActivePlayer(player);
//                        player.setClient(client);
//                        // Pass Player reference into thread
//                        client.setPlayer(player);
//                        // Set response information
//                        responseLogin.setStatus((short) 0); // Login is a success
//                        responseLogin.setPlayer(player);

//                        Log.printf("User '%s' has successfully logged in.", player.getUsername());
//                    }
//                }
//            }
//        } else {
//            responseLogin.setStatus((short) 3); // Client version not compatible
//            Log.printf("User '%s' has failed to log in. (v%s)", player.getUsername(), version);
//        }

  }
}
