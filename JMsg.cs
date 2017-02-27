using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Jproto;


public class JMsg  {

	static JMsg mMsg = null;
	public static JMsg instance{
		get{ 
			if (mMsg == null) {
				mMsg = new JMsg ();
			}
			return mMsg;
		}
	}

	private JMsg(){}

	public Dictionary<string,List<object>>msgDict = new Dictionary<string, List<object>>();
	//pkt handler
	public Dictionary<Jproto.PKT_TYPE,List< Action<byte[]>>>pktMsgDict = new Dictionary<Jproto.PKT_TYPE, List<Action<byte[]>>>();

	public void ListenPktMsg(PKT_TYPE pktType,Action<byte[]> f){
		if (pktMsgDict.ContainsKey (pktType) == false) {
			pktMsgDict [pktType] = new List<Action<byte[]>>{ f };
		} else {
			pktMsgDict [pktType].Add (f);
		}
	}

	public void RemovePktMsg(PKT_TYPE pktType,Action<byte[]>f){
		if (pktMsgDict.ContainsKey (pktType) == false) {
			return;
		} else {
			pktMsgDict [pktType].Remove (f);
		}
	}

	public void SendPktMsg(PKT_TYPE pktType,byte[] data){
		if (pktMsgDict.ContainsKey (pktType)) {
			List<Action<byte[]>> fList = new List<Action<byte[]>> (); 
			foreach (var f in pktMsgDict[pktType]) {
				fList.Add (f);
			}	
			foreach (var f in fList) {
				f (data);
			}
		}
	}


	public void ListenMsg(object obj,Action action){
		var key = obj.ToString ();
		if (msgDict.ContainsKey (key) == false) {
			msgDict [key] = new List<object> ();
		}
		if(msgDict [key].Contains(action)==false)
			msgDict [key].Add (action);
	}

	public void ListenMsg<T>(object obj,Action<T> action){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			msgDict[key] = new List<object> ();
		}
		if(msgDict [key].Contains(action)==false)
			msgDict [key].Add (action);
	}

	public void ListenMsg<T1,T2>(object obj,Action<T1,T2> action){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			msgDict [key] = new List<object> ();
		}
		if(msgDict [key].Contains(action)==false)
			msgDict [key].Add (action);
	}

	public void RemoveMsg(object obj,Action action){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		msgDict [key].Remove (action);
	}

	public void RemoveMsg<T>(object obj,Action<T> action){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		msgDict [key].Remove (action);
	}

	public void RemoveMsg<T1,T2>(object obj,Action<T1,T2> action){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		msgDict [key].Remove (action);
	}


	public void SendMsg(object obj){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		List<Action> fList = new List<Action> ();
		foreach (var func in msgDict[key]) {
			if (func is Action) {
				Action action = (Action)func;
				fList.Add (action);
			}
		}
		foreach (var f in fList) {
			f ();
		}
	}

	public void SendMsg<T>(object obj,T t){
		string key = obj.ToString();
		Debug.Log ("[SNED_MSG]" + typeof(T).ToString () + " " + key);
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		List<Action<T>> aList = new List<Action<T>> ();
		var tmList = msgDict [key];
		for (int i = 0; i < tmList.Count; i++) {
			var f = tmList [i];
			if (f is Action<T>) {
				Action<T> action = (Action<T>)f;
				aList.Add (action);
			}
		}
		foreach (var func in aList) {
			func (t);
		}
	}

	public void SendMsg<T1,T2>(object obj,T1 t1,T2 t2){
		string key = obj.ToString();
		if (msgDict.ContainsKey (key) == false) {
			return;
		}
		List<Action<T1,T2>> fList = new List<Action<T1, T2>> ();
		foreach (var func in msgDict[key]) {
			if (func is Action<T1,T2>) {
				Action<T1,T2> action = (Action<T1,T2>)func;
				fList.Add (action);
			}
		}
		foreach (var f in fList) {
			f (t1, t2);
		}
	}
}