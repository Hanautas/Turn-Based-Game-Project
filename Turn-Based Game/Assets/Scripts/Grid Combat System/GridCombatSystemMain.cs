using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;
using GridPathfindingSystem;
using System.Linq;

public class GridCombatSystemMain : MonoBehaviour {
    private GameObject[] playerObjectArray;
    private GameObject[] enemyObjectArray;
    public UnitGridCombat[] playerUnitGridCombatArray;
    public UnitGridCombat[] enemyUnitGridCombatArray;
    public UnitGridCombat[] unitGridCombatArray;

    private State state;
    public UnitGridCombat unitGridCombat;
    private List<UnitGridCombat> blueTeamList;
    private List<UnitGridCombat> redTeamList;
    private int blueTeamActiveUnitIndex;
    private int redTeamActiveUnitIndex;
    private bool canMoveThisTurn;
    private bool canAttackThisTurn;

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
        blueTeamActiveUnitIndex = -1;
        redTeamActiveUnitIndex = -1;

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

        SelectNextActiveUnit();
        UpdateValidMovePositions();
    }

    private void SelectNextActiveUnit() {
        if (unitGridCombat == null || unitGridCombat.GetTeam() == UnitGridCombat.Team.Red) {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Blue);
        } else {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Red);
        }

        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }

    private UnitGridCombat GetNextActiveUnit(UnitGridCombat.Team team) {
        if (team == UnitGridCombat.Team.Blue) {
            blueTeamActiveUnitIndex = (blueTeamActiveUnitIndex + 1) % blueTeamList.Count;
            if (blueTeamList[blueTeamActiveUnitIndex] == null || blueTeamList[blueTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return blueTeamList[blueTeamActiveUnitIndex];
            }
        } else {
            redTeamActiveUnitIndex = (redTeamActiveUnitIndex + 1) % redTeamList.Count;
            if (redTeamList[redTeamActiveUnitIndex] == null || redTeamList[redTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return redTeamList[redTeamActiveUnitIndex];
            }
        }
    }

    private void UpdateValidMovePositions() {
        GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        int maxMoveDistance = 5;
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
                if (Input.GetMouseButtonDown(0) && UtilitiesClass.IsPointerOverUIObject() == false) {
                    GridSystem<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(UtilitiesClass.GetMouseWorldPosition());

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
                                        TestTurnOver();
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
                                UpdateValidMovePositions();
                                TestTurnOver();
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

        // Flip team blue unit icon with F key
        if (Input.GetKeyDown(KeyCode.F) && unitGridCombat.team == UnitGridCombat.Team.Blue && unitGridCombat.unitIcon.transform.rotation.y == 0)
        {
            unitGridCombat.unitIcon.transform.Rotate(0, 180, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F) && unitGridCombat.team == UnitGridCombat.Team.Blue && unitGridCombat.unitIcon.transform.rotation.y != 0)
        {
            unitGridCombat.unitIcon.transform.Rotate(0, -180, 0);
        }
    }

    private void TestTurnOver() {
        if (!canMoveThisTurn && !canAttackThisTurn) {
            // Cannot move or attack, turn over
            ForceTurnOver();
        }
    }

    public void ForceTurnOver() {
        SelectNextActiveUnit();
        UpdateValidMovePositions();
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