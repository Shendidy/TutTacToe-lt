using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop3x3 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static DragAndDrop3x3 instance;
    #region Class Variables
    public GameObject micButton;
    public GameObject muteButton;
    public GameObject gameItemsPanel;
    public GameObject keyErrorPanel;
    public GameObject collectKeysPanel;
    private RectTransform rectTransform;
    public Text keyCount;
    public Text gameStatus;
    public Text playerScoreText;
    public Text cpuScoreText;
    [SerializeField] private Canvas mainCanvas;
    public Transform boardCanvas;
    private CanvasGroup canvasGroup;
    public CanvasGroup canvasGroup11;
    public CanvasGroup canvasGroup12;
    public CanvasGroup canvasGroup13;
    public CanvasGroup canvasGroup21;
    public CanvasGroup canvasGroup22;
    public CanvasGroup canvasGroup23;
    public Rigidbody2D nodeTopRight;
    public Rigidbody2D nodeBottomLeft;
    private Slot pieceStartSlot;
    private Rigidbody2D[] players;
    private List<Slot> playersLocation;
    private Rigidbody2D newComputerPiece;
    private Slot newComputerSlot;
    // Slots
    public Rigidbody2D slot11;
    public Rigidbody2D slot21;
    public Rigidbody2D slot31;
    public Rigidbody2D slot12;
    public Rigidbody2D slot22;
    public Rigidbody2D slot32;
    public Rigidbody2D slot13;
    public Rigidbody2D slot23;
    public Rigidbody2D slot33;
    // Players
    public Rigidbody2D player11;
    public Rigidbody2D player12;
    public Rigidbody2D player13;
    public Rigidbody2D player21;
    public Rigidbody2D player22;
    public Rigidbody2D player23;
    // Audio
    public AudioSource shh;
    public AudioSource tick;

    private bool isOutOfBoard;
    private float canvasDotAlpha;
    private int keysTotal;
    private int difficulty;
    #endregion
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        keysTotal = GameDataManager.LoadGameData()._keys;
        GameManager.keysTotal = keysTotal;

        GameManager.isMicOn = !AudioListener.pause;
        micButton.SetActive(GameManager.isMicOn);
        muteButton.SetActive(!GameManager.isMicOn);

        keyCount.text = keysTotal < 0 ? "0" : keysTotal.ToString();
        gameStatus.text = "Pick a difficulty, then move a blue TUT to start the game";
        GameManager.newGame = true;
        GameManager.playerInTurn = 1;
        GameManager.gameOver = false;

        GameManager.difficulty =
            GameManager.difficulty == 2 ? 2 : GameManager.difficulty == 3 ? 3 : 1;

        difficulty = GameManager.difficulty;

        GameManager.boardWidth = 3;

        SetPlayersMovedService.FirstSetupPlayersMoved();

        GameManager.playerScore = GameManager.cpuScore = 0;
    }

    private void Start()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver && keysTotal >= 0)
        {
            AdMob.instance.RequestInterstitial();
            shh.Play();
            if (GameManager.newGame)
            {
                PopulateSlotCentres();
                AddSlotCentresOccupiers();
                GameManager.newGame = false;

                if (GameManager.interstitialAdCounter >= 2 && keysTotal > 0)
                {
                    AdMob.instance.RequestInterstitial();
                }
            }
            pieceStartSlot = GameManager.boardSlots3x3.Where(slot => (slot.SOccupier?.name == rectTransform.name)).ToArray()[0];

            canvasDotAlpha = canvasGroup.alpha;
            canvasGroup.alpha = Constants._MovingPlayerTransparency;

            players = PopulateService.PopulatePlayers(new Rigidbody2D[]{player11, player12, player13, player21, player22, player23});
            PopulatePlayersLocation();
        }
        if (keysTotal < 0)
        {
            if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
            if (keyErrorPanel != null) keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver && GameManager.keysTotal >= 0)
        {
            rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.gameOver && GameManager.keysTotal >= 0)
        {
            AdMob.instance.RequestRewardedAd();
            var playerPiece = rectTransform.name.ToCharArray()[6].ToString() == "1";
            isOutOfBoard = ChecksService.CheckIfOutOfBoard(rectTransform.position, nodeTopRight.position, nodeBottomLeft.position);
            if (!isOutOfBoard && playerPiece) gameStatus.text = "";
            MovePiece(isOutOfBoard);
            tick.Play();
        }
    }
    private Slot GetFinalSlot()
    {
        float distance = float.MaxValue;
        Slot finalSlot = new Slot("temp", new Vector2(0, 0));

        foreach (Slot slot in GameManager.boardSlots3x3)
            if (Vector2.Distance(rectTransform.position, slot.SVector2) < distance)
            {
                distance = Vector2.Distance(rectTransform.position, slot.SVector2);
                finalSlot = slot;//don't attempt to break out of foreach after this step!
            }

        return finalSlot;
    }
    private void MovePiece(bool isOutOfBoard)
    {
        Slot finalSlot = GetFinalSlot();

        if (isOutOfBoard || !ChecksService.SlotIsFree(finalSlot) || !ValidPlayer())
        {
            rectTransform.position = pieceStartSlot.SVector2;
            canvasGroup.alpha = canvasDotAlpha;
        }
        else
        {
            Rigidbody2D currentPlayer = GetCurrentPiece();
            rectTransform.position = finalSlot.SVector2;

            SetPlayersMovedService.SetPlayersMoved3x3(rectTransform);

            foreach (Slot slot in GameManager.boardSlots3x3)
            {
                if (slot.SOccupier?.name == rectTransform.name)
                {
                    slot.SOccupier = null;
                    break;
                }
            }

            foreach (Slot slot in GameManager.boardSlots3x3)
            {
                if (Vector2.Distance(slot.SVector2, rectTransform.position) < 0.25)
                {
                    slot.SOccupier = currentPlayer;
                    break;
                }
            }

            canvasGroup.alpha = Constants._StandingPlayerTransparency;

            if (ChecksService.IsWinner(GameManager.boardSlots3x3))
            {
                GameManager.playerScore++;
                playerScoreText.text = GameManager.playerScore.ToString();

                Debug.Log($"Player: {GameManager.playerScore}, CPU: {GameManager.cpuScore}");

                if ((GameManager.difficulty == 1)
                    || (GameManager.difficulty == 2 && GameManager.playerScore >= Constants._MidScore)
                    || (GameManager.difficulty == 3 && GameManager.playerScore >= Constants._HardScore))
                {
                    GameManager.gameOver = true;
                    gameStatus.color = Color.blue;
                    gameStatus.text = "YOU WIN!";

                    GameManager.interstitialAdCounter++;

                    FireworksToggle.instance.ToggleFireworksPanel();
                }
            }
            if(!GameManager.gameOver)
            {
                ChangeAction.ChangePlayerInTurn();
                ComputerMove();
            }
        }
    }
    private Rigidbody2D GetCurrentPiece()
    {
        Rigidbody2D currentPlayer = new Rigidbody2D();
        foreach (Rigidbody2D player in players)
            if (player.name == rectTransform.name) currentPlayer = player;
        return currentPlayer;
    }
    private void PopulateSlotCentres()
    {
        GameManager.boardSlots3x3 = new Slot[9];

        GameManager.boardSlots3x3[0] = new Slot("slot11", slot11.position);
        GameManager.boardSlots3x3[1] = new Slot("slot21", slot21.position);
        GameManager.boardSlots3x3[2] = new Slot("slot31", slot31.position);
        GameManager.boardSlots3x3[3] = new Slot("slot12", slot12.position);
        GameManager.boardSlots3x3[4] = new Slot("slot22", slot22.position);
        GameManager.boardSlots3x3[5] = new Slot("slot32", slot32.position);
        GameManager.boardSlots3x3[6] = new Slot("slot13", slot13.position);
        GameManager.boardSlots3x3[7] = new Slot("slot23", slot23.position);
        GameManager.boardSlots3x3[8] = new Slot("slot33", slot33.position);
    }
    private void AddSlotCentresOccupiers()
    {
        if (GameManager.newGame)
        {
            GameManager.boardSlots3x3[0].SOccupier = player11;
            GameManager.boardSlots3x3[1].SOccupier = player12;
            GameManager.boardSlots3x3[2].SOccupier = player13;
            GameManager.boardSlots3x3[6].SOccupier = player21;
            GameManager.boardSlots3x3[7].SOccupier = player22;
            GameManager.boardSlots3x3[8].SOccupier = player23;
        }
    }
    private void PopulatePlayersLocation()
    {
        playersLocation = new List<Slot>();
        foreach (Slot slot in GameManager.boardSlots3x3)
        {
            if (slot.SOccupier) playersLocation.Add(slot);
        }
    }
    private bool ValidPlayer() =>
        rectTransform.name.ToCharArray()[6].ToString() == GameManager.playerInTurn.ToString();
    public void ComputerMove()
    {
        SetDifficultyFromButtons();

        if (difficulty == 1)
        {
            newComputerSlot = PickAction.PickRandomSlot();
            newComputerPiece = PickAction.PickComputerPiece(players);
        }
        else
        {
            int boardScore = int.MinValue;
            List<Rigidbody2D> computerPieceToMove = new List<Rigidbody2D>(); // Populate with final computer piece to move decision
            List<Slot> newSlot = new List<Slot>(); // Populate with final slot to move piece to
            Rigidbody2D[] computerPlayers = players.Where(player => (player.name.ToCharArray()[6].ToString() == "2")).ToArray();

            foreach (Rigidbody2D computerPiece in computerPlayers)
            {
                bool moved = ChecksService.CheckIfMoved(computerPiece);
                Vector2 oldPosition = computerPiece.position;

                if (difficulty == 2)
                {
                    Slot[] tempBoard = CopyService.CopySlotsArray(GameManager.boardSlots3x3);
                    Slot[] freeSlots = tempBoard.Where(slot => (slot.SOccupier == null)).ToArray();
                    foreach (Slot slot in freeSlots)
                    {
                        tempBoard = UpdateBoardSlotsService.UpdateBoardSlots(tempBoard, computerPiece, slot, false);
                        if (!moved) SetPlayersMovedService.SetPlayersMoved3x3(computerPiece);
                        int score = CalculationsService.CalculateBoardScore(tempBoard);
                        if (!moved) SetPlayersMovedService.SetPlayersNotMoved3x3(computerPiece);
                        if (score > boardScore)
                        {
                            computerPieceToMove = new List<Rigidbody2D>();
                            newSlot = new List<Slot>();
                            boardScore = score;
                            computerPieceToMove.Add(computerPiece);
                            newSlot.Add(slot);
                        }
                        else if (score == boardScore)
                        {
                            computerPieceToMove.Add(computerPiece);
                            newSlot.Add(slot);
                        }
                    }
                }
                else if (difficulty == 3) // Probably wont have this option!
                {
                    // put here the logic of checking all available moves in a depth of 2 check levels
                }
                computerPiece.position = oldPosition;
            }

            System.Random random = new System.Random();
            int i = random.Next(0, newSlot.Count);

            newComputerSlot = GameManager.boardSlots3x3.Where(slot => (slot.SName == newSlot[i].SName)).ToArray()[0];
            newComputerPiece = computerPieceToMove[i];
        }
        GameManager.boardSlots3x3 =
            UpdateBoardSlotsService.UpdateBoardSlots(GameManager.boardSlots3x3, newComputerPiece, newComputerSlot, true);

        // Make alpha of newPlayerPiece = 1
        switch (newComputerPiece.name)
        {
            case "Player2-1":
                canvasGroup21.alpha = 1;
                break;
            case "Player2-2":
                canvasGroup22.alpha = 1;
                break;
            default:
                canvasGroup23.alpha = 1;
                break;
        }
        if (ChecksService.IsWinner(GameManager.boardSlots3x3))
        {
            GameManager.cpuScore++;
            cpuScoreText.text = GameManager.cpuScore.ToString();

            if ((GameManager.difficulty == 1)
                || (GameManager.difficulty == 2 && GameManager.cpuScore >= Constants._MidScore)
                || (GameManager.difficulty == 3 && GameManager.cpuScore >= Constants._HardScore))
            {
                GameManager.gameOver = true;
                gameStatus.color = Color.red;
                gameStatus.text = "YOU LOSE!";

                GameManager.interstitialAdCounter++;
                if (GameManager.interstitialAdCounter >= Constants._InterstitialAdFrequency)
                {
                    AdMob.instance.ShowInterstitialAd();
                }
            }
        }
        if (!GameManager.gameOver) ChangeAction.ChangePlayerInTurn();
    }

    private void SetDifficultyFromButtons()
    {
        System.Random rand = new System.Random();

        int i = rand.Next(0, 100);

        switch (GameManager.difficulty)
        {
            case 1:
                difficulty = i < 60 ? 2 : 1;
                break;
            case 2:
                difficulty = i < 105 ? 2 : 1;
                break;
            case 3:
                difficulty = i < 105  ? 2 : 1;
                break;
        }
    }
}