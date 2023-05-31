using AssemblyGameModel;
using NuGet.Frameworks;
using System.Globalization;
using System.Net.Http.Headers;
using Moq;

namespace AssemblyGameTest
{
    [TestClass]
    public class AssemblyGameModelTest
    {
        private GameModel _model = null;
        private Mock<IDataAccess> _mock = null!; // az adatelérés mock-ja

        [TestInitialize]

        public void Initialize()
        {
            // kezdünk egy új játékot
            _model = new GameModel();
            _model.startNewGame();
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    _model[i, j] = new Empty(new Position(i, j));
                }
                _model[0, i] = new Empty(new Position(0, i));
            }
            _model[0, 0] = new Road(new Position(0, 0));
            _mock = new Mock<IDataAccess>();
            IArea bArea;
            bArea = new Police(new Position(1, 2));
            _model.GameFieldChanged(bArea, 1, 2);
            bArea = new Residental(new Position(3, 4));
            _model.GameFieldChanged(bArea, 3, 4);
            bArea = new Forest(new Position(5, 6));
            _model.GameFieldChanged(bArea, 5, 6);

            _mock.Setup(mock => mock.LoadAsync(It.IsAny<string>()))
                .ReturnsAsync(_model);
            _mock.SetupAllProperties();
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállított játéktáblát fogja visszaadni
            // elõre definiálunk egy játéktáblát a perzisztencia mockolt teszteléséhez
        }

        [TestMethod]
        public void StartNewGameTest()
        {
            _model.startNewGame();
            Assert.IsFalse(_model.RemovingMode);
            Assert.IsFalse(_model.MetropolisMode);

            int forestCounter = 0;
            for(int i = 0; i < _model.HEIGHT; i++)
            {
                for(int j = 0; j < _model.WIDTH; j++)
                {
                    if (_model[i, j] is Forest)
                        forestCounter++;
                }
            }
            Assert.AreEqual(10, forestCounter);
            Assert.AreEqual("Road", _model[0, 0].Name);
            Assert.AreEqual(5000, _model.Money);
            Assert.AreEqual(0, _model.Population);
            Assert.AreEqual(Speed.Normal, _model.GameSpeed);
            Assert.AreEqual(0, _model.Tax);

        }

        [TestMethod]
        public void OutOfRangeIArea()
        {
            _model.startNewGame();
            try
            {
                if (_model[-1, 0] is Empty)
                { }
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException) { }

            try
            {
                if (_model[0, -1] is Empty)
                { }
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException) { }

        }


        [TestMethod]
        public void BuildingDown()
        {
            _model.startNewGame();

            //HighSchool
            IArea highSchool = new Education(new Position(0, 0), EducationLevel.HighSchool);
            try
            {
                _model.GameFieldChanged(highSchool, _model.HEIGHT - 1, _model.WIDTH - 1);
                Assert.Fail();
            }
            catch(ArgumentException) { }
            if (_model[0, 1] is Empty && _model[0, 2] is Empty)
            {
                _model.GameFieldChanged(highSchool, 0, 1);
                Assert.AreEqual("HighSchool", _model[0, 1].Name);
                Assert.AreEqual("HighSchool", _model[0, 2].Name);
            }


            //University
            IArea uni = new Education(new Position(0, 0), EducationLevel.University);
            try
            {
                _model.GameFieldChanged(uni, _model.HEIGHT - 1, _model.WIDTH - 1);
                Assert.Fail();
            }
            catch (ArgumentException) { }
            try
            {
                _model.GameFieldChanged(uni, _model.HEIGHT - 2, _model.WIDTH - 1);
                Assert.Fail();
            }
            catch (ArgumentException) { }
            try
            {
                _model.GameFieldChanged(uni, _model.HEIGHT - 1, _model.WIDTH - 2);
                Assert.Fail();
            }
            catch (ArgumentException) { }

            try
            {
                _model.GameFieldChanged(uni, 0, 0);
                Assert.Fail();
            }
            catch (ArgumentException) { }

            if (_model[2, 2] is Empty && _model[2, 3] is Empty && _model[3, 2] is Empty && _model[3, 3] is Empty)
            {
                _model.GameFieldChanged(uni, 2, 2);
                Assert.AreEqual("University", _model[2, 2].Name);
                Assert.AreEqual("University", _model[2, 3].Name);
                Assert.AreEqual("University", _model[3, 2].Name);
                Assert.AreEqual("University", _model[3, 3].Name);
            }
        }

        [TestMethod]
        public void BuildingRemove()
        {
            _model.startNewGame();
            _model.RemovingMode = true;
            
            try
            {
                _model.GameFieldChanged(new Empty(new Position(0, 0)), 0, 0);
            }
            catch(ArgumentException) { }

            //Residental down and remove
            _model.RemovingMode = false;
            _model.GameFieldChanged(new Residental(new Position(2, 2)), 2, 2);
            ((Residental)_model[2, 2]).Fullness = 1;
            Assert.AreEqual(1, ((Residental)_model[2, 2]).Fullness);

            _model.RemovingMode = true;
            try
            {
                _model.GameFieldChanged(new Empty(new Position(2, 2)), 2, 2);
            }
            catch (ArgumentException) { }
            ((Residental)_model[2, 2]).Fullness = 0;
            _model.GameFieldChanged(new Empty(new Position(2, 2)), 2, 2);
            Assert.AreEqual("Empty", _model[2, 2].Name);


            //service down and remove
            _model.RemovingMode = false;
            _model.GameFieldChanged(new Service(new Position(2, 2)), 2, 2);
            ((Service)_model[2, 2]).Fullness = 1;
            Assert.AreEqual(1, ((Service)_model[2, 2]).Fullness);

            _model.RemovingMode = true;
            try
            {
                _model.GameFieldChanged(new Empty(new Position(2, 2)), 2, 2);
            }
            catch (ArgumentException) { }
            ((Service)_model[2, 2]).Fullness = 0;
            _model.GameFieldChanged(new Empty(new Position(2, 2)), 2, 2);
            Assert.AreEqual("Empty", _model[2, 2].Name);
        }


        [TestMethod]
        public void BuildingMetropolis()
        {
            _model.startNewGame();
            IArea area = new Forest(new Position(0, 0));
            _model.GameFieldChanged(area, 0, 1);

            _model.GameFieldChanged(new Residental(new Position(2, 2)), 2, 2);
            _model.MetropolisMode = true;


            try
            {
                _model.GameFieldChanged(_model[0, 1], 0, 1);
            }
            catch (Exception) { }
            try
            {
                _model.GameFieldChanged(_model[2, 2], 2, 2);
            }
            catch(Exception) { }

            ((Residental)_model[2, 2]).Fullness = 1;
            ((Residental)_model[2, 2]).Metropolis = MetropolisLevel.Level1;
            _model.GameFieldChanged(_model[2, 2], 2, 2);
            Assert.AreEqual(4650, _model.Money);
            Assert.AreEqual(MetropolisLevel.Level2, ((Residental)_model[2, 2]).Metropolis);
            Assert.AreEqual(4, ((Residental)_model[2, 2]).ImageId);
            Assert.AreEqual(40, ((Residental)_model[2, 2]).MaxPeople);

            _model.GameFieldChanged(_model[2, 2], 2, 2);
            Assert.AreEqual(4500, _model.Money);
            Assert.AreEqual(MetropolisLevel.Level3, ((Residental)_model[2, 2]).Metropolis);
            Assert.AreEqual(5, ((Residental)_model[2, 2]).ImageId);
            Assert.AreEqual(70, ((Residental)_model[2, 2]).MaxPeople);

            try
            {
                _model.GameFieldChanged(_model[2, 2], 2, 2);
                Assert.Fail();
            }
            catch (Exception) { }

        }

        [TestMethod]
        public async Task GameModelLoadTest()
        {

            _model = new GameModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal


            // majd betöltünk egy játékot
            await _model.LoadGameAsync(String.Empty);

            for (Int32 i = 0; i < 20; i++)
                for (Int32 j = 0; j < 31; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        Assert.AreEqual("Road", _model[i, j].Name);
                    }
                    else if (i == 1 && j == 2)
                    {
                        Assert.AreEqual("Police", _model[i, j].Name);

                    }
                    else if (i == 3 && j == 4)
                    {
                        Assert.AreEqual("Residental", _model[i, j].Name);
                    }
                    else if (i == 5 && j == 6)
                    {
                        Assert.AreEqual("Forest", _model[i, j].Name);
                    }
                    else
                    {
                        Assert.AreEqual("Empty", _model[i, j].Name);
                    }

                    // ellenõrizzük, valamennyi mezõ értéke megfelelõ-e
                }

            // ellenõrizzük, hogy meghívták-e a Load mûveletet a megadott paraméterrel
            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }
    }
}