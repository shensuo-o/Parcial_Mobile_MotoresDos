using Clients;
using UnityEngine;

namespace Gameplay
{
    public class Entrada : MonoBehaviour
    {
        private Seat[] _placesToStand;

        private void Start()
        {
            _placesToStand = GetComponentsInChildren<Seat>();
            foreach (var place in _placesToStand)
            {
                place.IsFree();
            }
        }

        public void SpawnClient(Client client)
        {
            foreach (var place in _placesToStand)
            {
                if (place.IsFree())
                {
                    client.GoToSeat(place);
                    break;
                }
            }
        }

        public bool PlaceAvailable()
        {
            foreach (Seat seat in _placesToStand)
            {
                if (seat.IsFree())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
