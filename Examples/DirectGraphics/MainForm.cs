using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SoftwareGraphics;
using SoftwareGraphics.Devices;
using SoftwareGraphics.Objects;

using GenericMathematics;

using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace DirectGraphics
{
	public partial class MainForm : Form
	{
		enum CameraCaptureState
		{
			None,
			Move,
			Rotate,
		}

		const float MoveByMouseSpeed = 30f;

		GraphicsDevice device;
		Viewport viewport;

		Scene scene;
		Camera camera;

		CameraCaptureState cameraCapture;
		Point mousePosition;

		ModelObject foo;
		Vector3 pipePart = Vector3.Zero;
		Vector3 pipePartNormal = Vector3.Forward;

		HyperGraphicsDevice hyperDevice;
		Matrix<float> hyperWorld;
		Matrix<float> hyperProjection;
		Matrix<float> hyperTranslation;

		List<HyperLine> hyperModel = new List<HyperLine>();

		public MainForm()
		{
			InitializeComponent();
			this.MouseWheel += MainForm_MouseWheel;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			camera = new Camera(new Vector3(0, 0, 10f));

			viewport = new Viewport(0, 0, panelGraphics.ClientSize.Width, panelGraphics.ClientSize.Height);

			//Matrix projection = MatrixHelper.CreateOrthographic(
			//    (float)viewport.Width, (float)viewport.Height, (float)0, (float)2000);
			Matrix projection = MatrixHelper.CreatePerspectiveFieldOfView(
				(float)Math.PI / 4, viewport.AspectRatio, 1, 200f);
            
			device = new GdiGraphicsDevice(panelGraphics.CreateGraphics(), viewport);

			device.CullMode = CullMode.None;
			device.IsWireframe = true;

			scene = new Scene(device, camera, projection);

			//var rainbow = ColorHelper.GetRainbow(Color.Red, 15f).GetEnumerator();
			//var rainbow = ColorHelper.GetFixedColors(
			//    new[] { Color.Red, Color.Yellow, Color.Green, Color.Orange }).GetEnumerator();
			var rainbow = ColorHelper.GetFixedColors(
				new[] { Color.Red, Color.White }).GetEnumerator();

			//foo = new PythagorasTree(scene, new Vector3(50, 0, 0), new Vector3(10, 30, 0), 1f);
			//foo = new Sphere(scene, 100, rainbow);
			foo = new Pipe(16, rainbow);

			float xScale = (float)panelGraphics.ClientSize.Width / 2;
			float yScale = (float)panelGraphics.ClientSize.Height / 2;
			float zScale = (xScale + yScale) / 2f;

			hyperDevice = new HyperGraphicsDevice(scene);
			hyperProjection = HyperMatrix.CreatePerspectiveFieldOfView((float)Math.PI / 4, 0.01f, 10f);
			hyperWorld = HyperMatrix.CreateIdentity();

			GenerateHyperCube(hyperModel, ref hyperWorld, ref hyperTranslation);

			//var colorer = ColorHelper.GetRainbow(Color.Red, 10f).GetEnumerator();
			//for (int i = 0; i < hyperModel.Count; i++)
			//{
			//    colorer.MoveNext();

			//    var trangle = hyperModel[i];
			//    trangle.Color = colorer.Current;

			//    hyperModel[i] = trangle;
			//}

			Application.Idle += new EventHandler((s, ea) => UpdateScreen());
		}

		private static void GenerateHyperCube(
			IList<HyperLine> model, ref Matrix<float> world, ref Matrix<float> translation)
		{
			float m = 2f;

			Vector4[] points = new[]
			{
				new Vector4(0, 0, 0, 0),
				new Vector4(m, 0, 0, 0),
				new Vector4(0, m, 0, 0),
				new Vector4(0, 0, m, 0),
				new Vector4(m, m, 0, 0),
				new Vector4(0, m, m, 0),
				new Vector4(m, 0, m, 0),
				new Vector4(m, m, m, 0),
			};

			var hyperPoints = points.Select(v => new Vector4(v.X, v.Y, v.Z, m));
			var p = points.Concat(hyperPoints).ToArray();

			//        2------4
			//       /|     /|       
			//      5------7 |       Y
			//      | |    | |       |
			//      | 0----|-1       #-- X
			//      |/     |/       /
			//      3------6       Z

			int[,] indices = new int[,]
			{
				{0, 1}, {1, 6}, {6, 3}, {3, 0},
				{2, 4}, {4, 7}, {7, 5}, {5, 2},
				{0, 2}, {1, 4}, {6, 7}, {3, 5},
			};

			int baseIndex = points.Length;

			for (int i = 0; i < indices.GetLength(0); i++)
			{
				int startIndex = indices[i, 0];
				int endIndex = indices[i, 1];

				model.Add(new HyperLine(p[startIndex], p[endIndex], Color.LightGreen));
				model.Add(new HyperLine(p[startIndex + baseIndex], p[endIndex + baseIndex], Color.Cyan));
			}

			for (int i = 0; i < baseIndex; i++)
			{
				model.Add(new HyperLine(p[i], p[i + baseIndex], Color.LightPink));
			}

			world *= HyperMatrix.CreateTranslation(new Vector4(-m / 2));
			translation = HyperMatrix.CreateTranslation(new Vector4(0, 0, 0, 2.5f));
		}

		private static void GenerateHyperSphere(
			IList<HyperLine> model, ref Matrix<float> world, ref Matrix<float> translation)
		{
			throw new NotImplementedException();
		}

		private void UpdateScreen()
		{
			hyperDevice.Clear(Color.CornflowerBlue);
			hyperDevice.Begin();

			var wvp = hyperWorld * hyperTranslation * hyperProjection;

			foreach (var triangle in hyperModel)
			{
				hyperDevice.Draw(triangle, wvp);
			}

			hyperDevice.End();
		}

#pragma warning disable 0162

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			const float speed = 10f;

			switch (e.KeyCode)
			{
				case Keys.X:
					hyperWorld *= HyperMatrix.CreateRotationXY(0.1f);
					break;
				case Keys.Y:
					hyperWorld *= HyperMatrix.CreateRotationYZ(0.1f);
					break;
				case Keys.Z:
					hyperWorld *= HyperMatrix.CreateRotationXZ(0.1f);
					break;
			}
			return;

			Pipe pipe = foo as Pipe;
			if (pipe != null)
			{
				const float partLength = 5f;

				Vector3 lastNormal = pipePartNormal;

				switch (e.KeyCode)
				{
					case Keys.Left:
						pipePartNormal = Vector3.Left;
						break;
					case Keys.Right:
						pipePartNormal = Vector3.Right;
						break;
					case Keys.Up:
						pipePartNormal = Vector3.Forward;
						break;
					case Keys.Down:
						pipePartNormal = Vector3.Backward;
						break;
					case Keys.PageUp:
						pipePartNormal = Vector3.Up;
						break;
					case Keys.PageDown:
						pipePartNormal = Vector3.Down;
						break;
					default:
						break;
				}

				pipePart += (pipePartNormal + lastNormal) * partLength;
				pipe.AddPart(pipePart, pipePartNormal,
					(pipePartNormal == Vector3.Up ||
					pipePartNormal == Vector3.Down
					? Vector3.Right : Vector3.Up) * 10f);

				UpdateScreen();
				return;
			}

			DrawableObject currentObject = foo;

			if (!e.Shift) // Absolute
			{
				switch (e.KeyCode)
				{
					case Keys.Left:
						currentObject.MoveBy(new Vector3(-speed, 0, 0));
						break;

					case Keys.Right:
						currentObject.MoveBy(new Vector3(speed, 0, 0));
						break;

					case Keys.Up:
						currentObject.MoveBy(new Vector3(0, speed, 0));
						break;

					case Keys.Down:
						currentObject.MoveBy(new Vector3(0, -speed, 0));
						break;

					case Keys.Q:
						currentObject.MoveBy(Vector3.Forward * speed);
						break;

					case Keys.E:
						currentObject.MoveBy(Vector3.Backward * speed);
						break;

					case Keys.Oemcomma: // <
						currentObject.Rotate(0.1f, 0, 0);
						break;

					case Keys.OemPeriod: // >
						currentObject.Rotate(-0.1f, 0, 0);
						break;

					case Keys.OemOpenBrackets: // [
						currentObject.Rotate(0, 0.1f, 0);
						break;

					case Keys.OemCloseBrackets: // ]
						currentObject.Rotate(0, -0.1f, 0);
						break;

					case Keys.PageUp:
						currentObject.Rotate(0, 0, 0.1f);
						break;

					case Keys.PageDown:
						currentObject.Rotate(0, 0, -0.1f);
						break;

					case Keys.G:
						if (currentObject is RandomSystem)
							((RandomSystem)currentObject).GeneratePart();
						else if (currentObject is PythagorasTree)
							((PythagorasTree)currentObject).GenerateNextLayer();
						else if (currentObject is PrismTree)
							((PrismTree)currentObject).GenerateNextLayer();
						else if (currentObject is PhysicsLab22)
							((PhysicsLab22)currentObject).SwitchSeries();
						break;
				}
			}
			else // Relative
			{
				switch (e.KeyCode)
				{
					case Keys.Left:
						currentObject.RelativeMoveBy(new Vector3(-speed, 0, 0));
						break;

					case Keys.Right:
						currentObject.RelativeMoveBy(new Vector3(speed, 0, 0));
						break;

					case Keys.Up:
						currentObject.RelativeMoveBy(new Vector3(0, speed, 0));
						break;

					case Keys.Down:
						currentObject.RelativeMoveBy(new Vector3(0, -speed, 0));
						break;

					case Keys.Q:
						currentObject.RelativeMoveBy(new Vector3(0, 0, -speed));
						break;

					case Keys.E:
						currentObject.RelativeMoveBy(new Vector3(0, 0, speed));
						break;

					case Keys.Oemcomma: // <
						currentObject.RelativeRotate(0.1f, 0, 0);
						break;

					case Keys.OemPeriod: // >
						currentObject.RelativeRotate(-0.1f, 0, 0);
						break;

					case Keys.OemOpenBrackets: // [
						currentObject.RelativeRotate(0, 0.1f, 0);
						break;

					case Keys.OemCloseBrackets: // ]
						currentObject.RelativeRotate(0, -0.1f, 0);
						break;

					case Keys.PageUp:
						currentObject.RelativeRotate(0, 0, 0.1f);
						break;

					case Keys.PageDown:
						currentObject.RelativeRotate(0, 0, -0.1f);
						break;
				}
			}

			UpdateScreen();
		}

#pragma warning restore 0162

        private void panelGraphics_MouseDown(object sender, MouseEventArgs e)
		{
			if (cameraCapture != CameraCaptureState.None)
				return;

			mousePosition = e.Location;

			if (e.Button == MouseButtons.Middle)
			{
				cameraCapture = CameraCaptureState.Move;
				this.Cursor = Cursors.SizeAll;
			}
			else if (e.Button == MouseButtons.Right)
			{
				cameraCapture = CameraCaptureState.Rotate;
				this.Cursor = Cursors.Cross;
			}
		}

		private void panelGraphics_MouseMove(object sender, MouseEventArgs e)
		{
			if (cameraCapture == CameraCaptureState.Move)
			{
				float dx = (float)(e.X - mousePosition.X) / panelGraphics.ClientSize.Width;
				float dy = (float)(e.Y - mousePosition.Y) / panelGraphics.ClientSize.Height;
				mousePosition = e.Location;

				camera.RelativeMoveBy(new Vector3(
					-dx * MoveByMouseSpeed,
					dy * MoveByMouseSpeed,
					0));
			}
			else if (cameraCapture == CameraCaptureState.Rotate)
			{
				float dx = (float)(e.X - mousePosition.X) / panelGraphics.ClientSize.Width;
				float dy = (float)(e.Y - mousePosition.Y) / panelGraphics.ClientSize.Height;
				mousePosition = e.Location;

				camera.RotateInXZAround(Vector3.Zero, (float)Math.PI * 2 * dx);
				camera.RotateInYZAround(Vector3.Zero, (float)Math.PI * 2 * dy);

				UpdateScreen();
			}
		}

		private void panelGraphics_MouseUp(object sender, MouseEventArgs e)
		{
			cameraCapture = CameraCaptureState.None;
			this.Cursor = Cursors.Default;
		}

		private void MainForm_MouseWheel(object sender, MouseEventArgs e)
		{
			camera.Zoom(e.Delta * 0.05f);
		}
	}
}
