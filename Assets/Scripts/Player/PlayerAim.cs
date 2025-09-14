using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputManagerSO input;

    [Header("Camera")]
    [SerializeField] private Camera cameraMain;

    [Header("Arm Pivot")]
    [SerializeField] private Transform armPivot;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer playerTorsoRenderer;
    [SerializeField] private SpriteRenderer playerArmsRenderer;

    [Header("Corresponding Sprites")]
    [SerializeField] private List<Sprite> playerTorsoSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> playerArmSprites = new List<Sprite>();

    [Header("Settings")]
    [SerializeField] private float pixelsPerUnit = 64f;
    [SerializeField] private float deadZone = 0.35f;
    private Vector2 aimDirection;
    public Vector2 targetDir;

    public int currentDirIndex = 0;


    #region ENABLE-DISABLE InputEvents
    private void OnEnable()
    {
        input.OnLook += CentralInputHandling;
    }

    private void OnDisable()
    {
        input.OnLook -= CentralInputHandling;
    }
    #endregion

    private void LateUpdate()
    {
        Vector3 pos = armPivot.position;
        pos.x = Mathf.Round(pos.x * pixelsPerUnit) / pixelsPerUnit;
        pos.y = Mathf.Round(pos.y * pixelsPerUnit) / pixelsPerUnit;
        armPivot.position = pos;
    }

    public Vector2 HandleAiming()
    {
        if (targetDir.sqrMagnitude > 0.001f)
        {
            // Direct assignment instead of smoothing
            aimDirection = targetDir.normalized;

            // Get angle straight away
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            // --- Snap sprite to 8 directions ---
            int dirIndex = Mathf.FloorToInt((angle + 22.5f) / 45f) % 8;
            currentDirIndex = dirIndex;
            playerArmsRenderer.sortingOrder = currentDirIndex == 2 ? 1 : 3;
                playerArmsRenderer.sprite = playerArmSprites[dirIndex];
            playerTorsoRenderer.sprite = playerTorsoSprites[dirIndex];

            // --- Snappy arm rotation ---
            armPivot.rotation = Quaternion.Euler(0, 0, angle+90f);
        }
        return aimDirection;
    }

    #region INPUT HANDLING

    private void CentralInputHandling(InputAction.CallbackContext ctx)
    {
        var device = ctx.control.device;
        if (device is Mouse)
        {

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorld = (Vector2)cameraMain.ScreenToWorldPoint(screenPos);
            targetDir = (mouseWorld - (Vector2)transform.position).normalized;
        }
        else if (device is Gamepad)
        {

            Vector2 stickValue = ctx.ReadValue<Vector2>();
            if (stickValue.magnitude > deadZone)
                targetDir = stickValue.normalized;
        }

        // Debugging
        Debug.DrawLine(transform.position, transform.position + (Vector3)targetDir * 2f, Color.yellow);

    }
    #endregion
}
