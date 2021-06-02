using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;
using GridPathfindingSystem;
using System.Linq;

using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GridCombatSystemMain : MonoBehaviour {
    private GameObject[] playerObjectArray;
    private GameObject[] enemyObjectArray;
    public UnitGridCombat[] playerUnitGridCombatArray;
    private int playerUnitCount;
    public UnitGridCombat[] enemyUnitGridCombatArray;
    private int enemyUnitCount;
    public UnitGridCombat[] unitGridCombatArray;
    private int arrayIndex;

    [Header ("Unit")]
    public UnitGridCombat unitGridCombat;
    private State state;    
    private List<UnitGridCombat> blueTeamList;
    private List<UnitGridCombat> redTeamList;
    private bool canMoveThisTurn;
    private bool canAttackThisTurn;

    public int maxMoveDistance;
    public Vector2 aIMoveVector;

    [Header ("AI")]
    public Vector3[] offsets;
    public Vector3[] offsetPositions;
    public GameObject closestPlayer;
    public Vector3 closestPosition;

    [Header ("Gameplay")]
    public TilemapCollider2D tilemapCollider;
    public bool isAttacking;
    public bool isMoving;

    [Header ("Time")]
    public int turnCount;
    public float logTimeRemaining;
    public bool isLogTimer;


    [Header ("UI")]
    public GameObject blackBackground;
    public GameObject gameOverWin;
    public GameObject gameOverLost;
    public TurnOrder turnOrder;
    
    public Image[] actionButtons;
    public GameObject[] playerUI;

    private enum State {
        Normal,
        Waiting
    }

    private void Awake() {
        state = State.Normal;
    }

    private void Start() {
        blueTeamList = new List<UnitGridCombat>();
        redTeamList = new List<UnitGridCombat>();
        //blueTeamActiveUnitIndex = -1;
        //redTeamActiveUnitIndex = -1;

        // Add UnitGridCombat with tag "Player" and "Enemy" to 2 arrays
        // Source: https://answers.unity.com/questions/710833/using-getcomponent-with-an-array.html
        playerObjectArray = GameObject.FindGameObjectsWithTag("Player");
        playerUnitGridCombatArray = new UnitGridCombat[playerObjectArray.Length];
        for (int i = 0; i < playerObjectArray.Length; i++)
        {
            playerUnitGridCombatArray[i] = playerObjectArray[i].GetComponent<UnitGridCombat>();
        }

        enemyObjectArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemyUnitGridCombatArray = new UnitGridCombat[enemyObjectArray.Length];
        for (int i = 0; i < enemyObjectArray.Length; i++)
        {
            enemyUnitGridCombatArray[i] = enemyObjectArray[i].GetComponent<UnitGridCombat>();
        }

        // Merge player and enemy array
        unitGridCombatArray = playerUnitGridCombatArray.Concat(enemyUnitGridCombatArray).ToArray();

        // Set all UnitGridCombat on their GridPosition
        foreach (UnitGridCombat unitGridCombat in unitGridCombatArray) {
            GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition()).SetUnitGridCombat(unitGridCombat);

            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue) {
                blueTeamList.Add(unitGridCombat);
            } else {
                redTeamList.Add(unitGridCombat);
            }
        }

        //SelectNextActiveUnit();
        //UpdateValidMovePositions();

        canMoveThisTurn = true;
        canAttackThisTurn = true;

        turnCount = 1;
        logTimeRemaining = 5f;
        isLogTimer = false;
    }

    public void IsAttacking()
    {
        isAttacking = true;
        UpdateValidMovePositions(true);

        if (isMoving)
        {
            isMoving = false;
        }
    }

    public void IsMoving()
    {
        isMoving = true;
        UpdateValidMovePositions(false);

        if (isAttacking)
        {
            isAttacking = false;
        }
    }

    public void ActiveButtonHighlight(Image buttonImage)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i] == buttonImage)
            {
                buttonImage.color = new Color32(100, 200, 255, 255);
            }
            else
            {
                actionButtons[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void SelectNextActiveUnit()
    {
        if (arrayIndex >= unitGridCombatArray.Length - 1)
        {
            // If index is at end of array, reset index to 0
            arrayIndex = 0;
            Debug.Log("Unit index: " + arrayIndex);

            if (unitGridCombatArray[0] == null)
            {
                for (int i = arrayIndex; i < unitGridCombatArray.Length - 1; i++)
                {
                    if (unitGridCombatArray[i + 1] != null)
                    {
                        // Get the next index in array
                        // If next index is null, skip nulls until index is not null
                        arrayIndex = i + 1;
                        Debug.Log("Unit index: " + arrayIndex);
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = arrayIndex; i < unitGridCombatArray.Length - 1; i++)
            {
                if (unitGridCombatArray[i + 1] != null)
                {
                    // Get the next index in array
                    // If next index is null, skip nulls until index is not null
                    arrayIndex = i + 1;
                    Debug.Log("Unit index: " + arrayIndex);
                    break;
                }

                if (arrayIndex >= unitGridCombatArray.Length - 1 - i && unitGridCombatArray[unitGridCombatArray.Length - 1] == null)
                {
                    if (unitGridCombatArray[0] == null)
                    {
                        for (int j = arrayIndex; j < unitGridCombatArray.Length - 1; j++)
                        {
                            if (unitGridCombatArray[j + 1] != null)
                            {
                                // Get the next index in array
                                // If next index is null, skip nulls until index is not null
                                arrayIndex = j + 1;
                                Debug.Log("Unit index: " + arrayIndex);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // If index is at end of array and end of array is null, reset index to 0
                        arrayIndex = 0;
                        Debug.Log("Unit index: " + arrayIndex);
                    }
                }
            }
        }

        // Set the next unit in unit array to active unit
        unitGridCombat = unitGridCombatArray[arrayIndex];

        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;

        turnCount += 1;

        // Highlight the next active unit in the turn order
        turnOrder.ActiveUnitTurn();

        // Show UI for player units
        CurrentUnitUI();

        // Check game over
        GameOver();

        // Action buttons default color
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].color = new Color32(255, 255, 255, 255);
        }
    }

    public void SortUnitArray()
    {
        unitGridCombatArray = unitGridCombatArray.OrderBy(x => x.GetComponent<UnitGridCombat>().initiative).ToArray();
        System.Array.Reverse(unitGridCombatArray);
        unitGridCombat = unitGridCombatArray[0];

        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());

        //SelectNextActiveUnit();
        //UpdateValidMovePositions();

        // Highlight current unit in turn order
        turnOrder.ActiveUnitTurn();

        // Show UI for player units
        CurrentUnitUI();
    }

    public void GameOver()
    {
        playerUnitCount = 0;
        for (int i = 0; i < playerUnitGridCombatArray.Length; i++)
        {
            if (playerUnitGridCombatArray[i] != null)
            {
                playerUnitCount += 1;
            }
        }
        if (playerUnitCount == 0)
        {
            gameOverLost.SetActive(true);
            blackBackground.SetActive(true);
            Debug.Log("Enemy wins.");
        }

        enemyUnitCount = 0;
        for (int i = 0; i < enemyUnitGridCombatArray.Length; i++)
        {
            if (enemyUnitGridCombatArray[i] != null)
            {
                enemyUnitCount += 1;
            }
        }
        if (enemyUnitCount == 0)
        {
            gameOverWin.SetActive(true);
            blackBackground.SetActive(true);
            Debug.Log("Player wins.");
        }
    }

    public void CurrentUnitUI()
    {
        // Hide player UI on enemy turn
        foreach (GameObject obj in playerUI)
        {
            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Red)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
            }
        }
    }

    private void UpdateValidMovePositions(bool attacking) {
        GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    grid.GetGridObject(x, y).SetIsValidMovePosition(false);
                }
            }
        }
        else if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Red)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    Vector3 tilemapPoint = new Vector3(x, y, 0);
                    if (tilemapCollider.bounds.Contains(tilemapPoint))
                    {
                        grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                    }
                    else if (!tilemapCollider.bounds.Contains(tilemapPoint))
                    {
                        grid.GetGridObject(x, y).SetIsValidMovePosition(false);
                    }
                }
            }
        }

        //maxMoveDistance = 5;
        if (attacking)
        {
            maxMoveDistance = unitGridCombat.attackRange + 1;
        }
        else
        {
            maxMoveDistance = (unitGridCombat.moveSpeed + 5) / 5;
        }
        
        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                if (gridPathfinding.IsWalkable(x, y)) {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        // There is a Path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance) {
                            // Path within Move Distance

                            // Set Tilemap Tile to Move
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                                x, y, MovementTilemap.TilemapObject.TilemapSprite.Move
                            );

                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        } else { 
                            // Path outside Move Distance!
                        }
                    } else {
                        // No valid Path
                    }
                } else {
                    // Position is not Walkable
                }
            }
        }
    }

    private void Update() {
        switch (state) {
            case State.Normal:
                if (unitGridCombat != null && Input.GetMouseButtonDown(0) && unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue) {
                    GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(UtilitiesClass.GetMouseWorldPosition());

                    if (isAttacking && canAttackThisTurn)
                    {
                        if (Input.GetMouseButtonDown(0) && UtilitiesClass.IsPointerOverUIObject() == false)
                        {
                            if (gridObject.GetIsValidMovePosition()) {
                                // Valid Move Position
                                // Check if clicking on a unit position
                                if (gridObject.GetUnitGridCombat() != null) {
                                    // Clicked on top of a Unit
                                    if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat())) {
                                        // Clicked on an Enemy of the current unit
                                        if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat())) {
                                            // Can Attack Enemy
                                            if (canAttackThisTurn) {
                                                canAttackThisTurn = false;
                                                // Attack Enemy
                                                state = State.Waiting;
                                                unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                                    state = State.Normal;
                                                    TurnOver();
                                                });
                                            }
                                        } else {
                                            // Cannot attack enemy
                                        }
                                        break;
                                    } else {
                                        // Not an enemy
                                    }
                                } else {
                                    // No unit here
                                }
                            }

                            isAttacking = false;
                        }
                    }

                    if (isMoving && canMoveThisTurn)
                    {
                        if (Input.GetMouseButtonDown(0) && UtilitiesClass.IsPointerOverUIObject() == false)
                        {
                            if (gridObject.GetIsValidMovePosition()) {
                                // Valid Move Position

                                if (canMoveThisTurn) {
                                    canMoveThisTurn = false;

                                    state = State.Waiting;

                                    // Set entire Tilemap to Invisible
                                    GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
                                        MovementTilemap.TilemapObject.TilemapSprite.None
                                    );

                                    // Remove Unit from current Grid Object
                                    grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                                    // Set Unit on target Grid Object
                                    gridObject.SetUnitGridCombat(unitGridCombat);

                                    unitGridCombat.MoveTo(UtilitiesClass.GetMouseWorldPosition(), () => {
                                        state = State.Normal;
                                        UpdateValidMovePositions(false);
                                        TurnOver();
                                    });
                                }
                            }

                            isMoving = false;
                        }
                    }
                }
                // Current unit is not a player, AI move action
                if (unitGridCombat != null && unitGridCombat.GetTeam() == UnitGridCombat.Team.Red)
                {
                    if (canMoveThisTurn || canAttackThisTurn)
                    {
                        FindClosestPlayer();
                    }

                    GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(aIMoveVector);

                    if (gridObject.GetIsValidMovePosition()) {
                        // Valid Move Position
                        // Check if clicking on a unit position
                        if (gridObject.GetUnitGridCombat() != null) {
                            // Clicked on top of a Unit
                            if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat())) {
                                // Clicked on an Enemy of the current unit
                                if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat())) {
                                    // Can Attack Enemy
                                    if (canAttackThisTurn) {
                                        canAttackThisTurn = false;
                                        // Attack Enemy
                                        state = State.Waiting;
                                        unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                            state = State.Normal;
                                            EnemyTurnOver();
                                            StartCoroutine(EnemyTurnOver());
                                        });
                                    }
                                } else {
                                    // Cannot attack enemy
                                }
                                break;
                            } else {
                                // Not an enemy
                            }
                        } else {
                            // No unit here
                        }
                    }

                    if (gridObject.GetIsValidMovePosition()) {
                        // Valid Move Position

                        if (canMoveThisTurn) {
                            canMoveThisTurn = false;

                            state = State.Waiting;

                            // Set entire Tilemap to Invisible
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
                                MovementTilemap.TilemapObject.TilemapSprite.None
                            );

                            // Remove Unit from current Grid Object
                            grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                            // Set Unit on target Grid Object
                            gridObject.SetUnitGridCombat(unitGridCombat);

                            unitGridCombat.MoveTo(aIMoveVector, () => {
                                state = State.Normal;
                                UpdateValidMovePositions(false);
                                //EnemyTurnOver();
                                StartCoroutine(EnemyTurnOver());
                            });
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    ForceTurnOver();
                }
                break;
            case State.Waiting:
                break;
        }

        // Flash color on the current unit while idle
        foreach (UnitGridCombat unit in unitGridCombatArray)
        {
            if (unit != null)
            {
                if (unit == unitGridCombat && unitGridCombat.GetState() == UnitGridCombat.State.Normal)
                {
                    unit.animator.SetBool("Active Unit", true);
                }
                else
                {
                    unit.animator.SetBool("Active Unit", false);
                }
            }
        }

        // Flip team blue unit icon with F key
        if (Input.GetKeyDown(KeyCode.F) && unitGridCombat.team == UnitGridCombat.Team.Blue && unitGridCombat.unitIcon.transform.rotation.y == 0)
        {
            unitGridCombat.unitIcon.transform.Rotate(0, 180, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F) && unitGridCombat.team == UnitGridCombat.Team.Blue && unitGridCombat.unitIcon.transform.rotation.y != 0)
        {
            unitGridCombat.unitIcon.transform.Rotate(0, -180, 0);
        }

        // Combat line spam prevention timer
        if (isLogTimer == true)
        {
            if (logTimeRemaining > 0)
            {
                logTimeRemaining -= Time.deltaTime;
            }
            else
            {
                logTimeRemaining = 0;
                isLogTimer = false;
            }
        }
    }

    void FindClosestPlayer()
    {
        float distanceToClosestPlayer = Mathf.Infinity;
        closestPlayer = null;
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject currentPlayer in allPlayers)
        {
            float distanceToPlayer = (currentPlayer.transform.position - unitGridCombat.transform.position).sqrMagnitude;
            if (distanceToPlayer < distanceToClosestPlayer)
            {
                distanceToClosestPlayer = distanceToPlayer;
                closestPlayer = currentPlayer;
            }
        }

        if (allPlayers.Length > 0)
        {
            float distance = Vector3.Distance(closestPlayer.transform.position, unitGridCombat.transform.position) + 1;

            if (distance > maxMoveDistance && distance != maxMoveDistance)
            {
                // Outside of maximum move distance, move closer to player
                FindClosestPosition();
                aIMoveVector = new Vector2(closestPosition.x, closestPosition.y);

                //Debug.Log("Distance: " + distance + ", target too far away");
            }
            else if (distance <= maxMoveDistance && distance > 2.5f)
            {
                // Inside maximum move distance, move next to player
                FindClosestPosition();
                aIMoveVector = new Vector2(closestPosition.x, closestPosition.y);

                //Debug.Log("Distance: " + distance + ", move closer to target");
            }
            else if (distance < 2.5f)
            {
                // Next to player, attack player
                aIMoveVector = new Vector2(closestPlayer.transform.position.x, closestPlayer.transform.position.y);

                //Debug.Log("Distance: " + distance + ", attack target");
            }
        }
        else
        {
            aIMoveVector = unitGridCombat.transform.position;

            //Debug.Log("No targets, moving to position " + aIMoveVector);
        }
    }

    // Find closest valid move position
    void FindClosestPosition()
    {
        float distanceToClosestPosition = Mathf.Infinity;

        for (int i = 0; i < offsets.Length; i++)
        {
            offsetPositions[i] = closestPlayer.transform.position + offsets[i];
        }

        foreach (Vector3 offsetPosition in offsetPositions)
        {
            float distanceToPosition = (offsetPosition - unitGridCombat.transform.position).sqrMagnitude;

            GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            GridObject gridObject = grid.GetGridObject(offsetPosition);

            if (gridObject.GetIsValidMovePosition())
            {
                if (distanceToPosition < distanceToClosestPosition)
                {
                    distanceToClosestPosition = distanceToPosition;
                    closestPosition = offsetPosition;
                }
            }
        }
    }

    private void TurnOver()
    {
        if (!canMoveThisTurn && !canAttackThisTurn)
        {
            // Cannot move or attack, turn over
            ForceTurnOver();
        }
    }

    private IEnumerator EnemyTurnOver()
    {
        if (!canMoveThisTurn && !canAttackThisTurn)
        {
            // Cannot move or attack, turn over
            yield return new WaitForSeconds(2f);
            ForceTurnOver();
        }
        
        if (canMoveThisTurn && !canAttackThisTurn)
        {
            // Cannot move or attack, turn over
            yield return new WaitForSeconds(2f);
            ForceTurnOver();
        }
    }

    public void ForceTurnOver() {
        SelectNextActiveUnit();
        UpdateValidMovePositions(false);
    }

    public class GridObject {

        private GridSystem<GridObject> grid;
        private int x;
        private int y;
        private bool isValidMovePosition;
        private UnitGridCombat unitGridCombat;

        public GridObject(GridSystem<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetIsValidMovePosition(bool set) {
            isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition() {
            return isValidMovePosition;
        }

        public void SetUnitGridCombat(UnitGridCombat unitGridCombat) {
            this.unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public UnitGridCombat GetUnitGridCombat() {
            return unitGridCombat;
        }
    }
}