using UnityEngine;

public class PlayerInputProvider : MonoBehaviour
{

    [SerializeField]
    private KartController kart = null;

    private void Update()
    {
        if (kart == null)
        {
            return;
        }

        // ZAS: If we are accelerating, tell the kart to accelerate
        if (Input.GetButton("Fire1"))
            kart.Accelerate();

        if (Input.GetButton("Jump"))
            kart.Jump();

        // ZAS: Tell the kart how to steer each update
        float horizontalMovement = Input.GetAxis("Horizontal");
        kart.Steer(horizontalMovement);
    }

}
