using UnityEngine;
using System.Collections;

public class ProximityDetector
: LayerSelector
{
	public enum NotificationMode
	{
		  SELF = 0
		, SELF_AND_CHILDREN
		, PARENT
		, PARENT_AND_SIBLINGS
	}

	private static readonly string MESSAGE_PROXIMITY_ENTER = "OnProximityEnter";
	private static readonly string MESSAGE_PROXIMITY_EXIT = "OnProximityExit";

	public NotificationMode m_NotificationMode;
	public bool m_RequireReceiver;

	void OnTriggerEnter2D( Collider2D collider )
	{
		if( base.IsOnTargetLayer( collider.gameObject ) )
		{
			Notify( MESSAGE_PROXIMITY_ENTER, collider.gameObject );
		}
	}

	void OnTriggerExit2D( Collider2D collider )
	{
		if( base.IsOnTargetLayer( collider.gameObject ) )
		{
			Notify( MESSAGE_PROXIMITY_EXIT, collider.gameObject );
		}
	}

	void Notify( string message, GameObject detectedObject )
	{
		SendMessageOptions options = m_RequireReceiver? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver;

		switch( m_NotificationMode )
		{
			case NotificationMode.SELF:
			{
				gameObject.SendMessage( message, detectedObject, options );
				break;
			}

			case NotificationMode.SELF_AND_CHILDREN:
			{
				gameObject.BroadcastMessage( message, detectedObject, options );
				break;
			}

			case NotificationMode.PARENT:
			{
				if( null != transform.parent )
				{
					transform.parent.gameObject.SendMessage( message, detectedObject, options );
				}

				break;
			}

			case NotificationMode.PARENT_AND_SIBLINGS:
			{
				if( null != transform.parent )
				{
					transform.parent.gameObject.BroadcastMessage( message, detectedObject, options );
				}
				
				break;
			}
		}
	}
}
