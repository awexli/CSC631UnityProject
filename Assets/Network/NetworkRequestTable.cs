using UnityEngine;

using System;
using System.Collections.Generic;

public class NetworkRequestTable {

	public static Dictionary<short, Type> requestTable { get; set; }
	
	public static void init() {
		requestTable = new Dictionary<short, Type>();
		add(Constants.CMSG_AUTH, "RequestCreate");
		add(Constants.CMSG_HEARTBEAT, "RequestHeartbeat");
        add(Constants.CMSG_LOGIN, "RequestLogin");
        add(Constants.CMSG_PLAYERS, "RequestPlayers");
		add(Constants.CMSG_TEST, "RequestTest");
        add(Constants.CMSG_MOVE, "RequestMove");
        add(Constants.CMSG_READY, "RequestReady");
        add(Constants.CMSG_START, "RequestStart");
        add(Constants.CMSG_UNREADY, "RequestUnready");
        add(Constants.CMSG_CHAT, "RequestChat");
        add(Constants.CMSG_LIGHT, "RequestLight");
        add(Constants.CMSG_P2CORRECT, "RequestP2Correct");
        add(Constants.CMSG_P2INCORRECT, "RequestP2Incorrect");
        add(Constants.CMSG_TIMER, "RequestTopScore");
        add(Constants.CMSG_SAVESCORE, "RequestSaveScore");
    }
	
	public static void add(short request_id, string name) {
		requestTable.Add(request_id, Type.GetType(name));
	}
	
	public static NetworkRequest get(short request_id) {
		NetworkRequest request = null;
		
		if (requestTable.ContainsKey(request_id)) {
			request = (NetworkRequest) Activator.CreateInstance(requestTable[request_id]);
			request.request_id = request_id;
		} else {
			Debug.Log("Request [" + request_id + "] Not Found");
		}
		
		return request;
	}
}
