using UnityEngine;
using System.Collections;

public class PickItem : MonoBehaviour {
	public void OnPickedUp( PickupItem item ) {
		if( item.PickupIsMine ) {
			Debug.Log( "I picked up something. That's a score!" );
			PhotonNetwork.player.AddScore( 1 );
			//GameManager.Instance.ItemCnt++;
			GameManager.Instance.StageClear();
		} else {
			Debug.Log( "Someone else picked up something. Lucky!" );
		}
	}
}
