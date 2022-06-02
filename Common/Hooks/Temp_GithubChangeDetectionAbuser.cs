progress.Message = Lang.gen[20].Value;
int num28 = 0;
while ((double)num28 < num9)
{
int num29 = num3;
int num30 = num4;
int num31 = num;
int num32 = num2;
float value2 = (float)num28 / (float)num9;
progress.Set(value2);
bool flag6 = false;
int num33 = 0;
int num34 = 0;
int num35 = 0;
while (!flag6)
{
  flag6 = true;
  int num36 = Main.maxTilesX / 2;
  int num37 = 200;
  num33 = ((!WorldGen.drunkWorldGen) ? WorldGen.genRand.Next(num7, Main.maxTilesX - num7) : (flag2 ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.5), Main.maxTilesX - num7) : WorldGen.genRand.Next(num7, (int)((double)Main.maxTilesX * 0.5))));
  num34 = num33 - WorldGen.genRand.Next(200) - 100;
  num35 = num33 + WorldGen.genRand.Next(200) + 100;
  if (num34 < evilBiomeBeachAvoidance)
  {
    num34 = evilBiomeBeachAvoidance;
  }
  if (num35 > Main.maxTilesX - evilBiomeBeachAvoidance)
  {
    num35 = Main.maxTilesX - evilBiomeBeachAvoidance;
  }
  if (num33 < num34 + evilBiomeAvoidanceMidFixer)
  {
    num33 = num34 + evilBiomeAvoidanceMidFixer;
  }
  if (num33 > num35 - evilBiomeAvoidanceMidFixer)
  {
    num33 = num35 - evilBiomeAvoidanceMidFixer;
  }
  if (num33 > num36 - num37 && num33 < num36 + num37)
  {
    flag6 = false;
  }
  if (num34 > num36 - num37 && num34 < num36 + num37)
  {
    flag6 = false;
  }
  if (num35 > num36 - num37 && num35 < num36 + num37)
  {
    flag6 = false;
  }
  if (num33 > WorldGen.UndergroundDesertLocation.X && num33 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
  {
    flag6 = false;
  }
  if (num34 > WorldGen.UndergroundDesertLocation.X && num34 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
  {
    flag6 = false;
  }
  if (num35 > WorldGen.UndergroundDesertLocation.X && num35 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
  {
    flag6 = false;
  }
  if (num34 < dungeonLocation + num8 && num35 > dungeonLocation - num8)
  {
    flag6 = false;
  }
  if (num34 < num30 && num35 > num29)
  {
    num29++;
    num30--;
    flag6 = false;
  }
  if (num34 < num32 && num35 > num31)
  {
    num31++;
    num32--;
    flag6 = false;
  }
}
int num38 = 0;
for (int n = num34; n < num35; n++)
{
  if (num38 > 0)
  {
    num38--;
  }
  if (n == num33 || num38 == 0)
  {
    int num39 = (int)WorldGen.worldSurfaceLow;
    while ((double)num39 < Main.worldSurface - 1.0)
    {
      if (Main.tile[n, num39].active() || *Main.tile[n, num39].wall > 0)
      {
        if (n == num33)
        {
          num38 = 20;
          WorldGen.ChasmRunner(n, num39, WorldGen.genRand.Next(150) + 150, true);
          break;
        }
        if (WorldGen.genRand.Next(35) == 0 && num38 == 0)
        {
          num38 = 30;
          bool makeOrb = true;
          WorldGen.ChasmRunner(n, num39, WorldGen.genRand.Next(50) + 50, makeOrb);
          break;
        }
        break;
      }
      else
      {
        num39++;
      }
    }
  }
  int num40 = (int)WorldGen.worldSurfaceLow;
  while ((double)num40 < Main.worldSurface - 1.0)
  {
    if (Main.tile[n, num40].active())
    {
      int num41 = num40 + WorldGen.genRand.Next(10, 14);
      for (int num42 = num40; num42 < num41; num42++)
      {
        if ((*Main.tile[n, num42].type == 59 || *Main.tile[n, num42].type == 60) && n >= num34 + WorldGen.genRand.Next(5) && n < num35 - WorldGen.genRand.Next(5))
        {
          *Main.tile[n, num42].type = 0;
        }
      }
      break;
    }
    num40++;
  }
}
double num43 = Main.worldSurface + 40.0;
for (int num44 = num34; num44 < num35; num44++)
{
  num43 += (double)WorldGen.genRand.Next(-2, 3);
  if (num43 < Main.worldSurface + 30.0)
  {
    num43 = Main.worldSurface + 30.0;
  }
  if (num43 > Main.worldSurface + 50.0)
  {
    num43 = Main.worldSurface + 50.0;
  }
  i2 = num44;
  bool flag7 = false;
  int num45 = (int)WorldGen.worldSurfaceLow;
  while ((double)num45 < num43)
  {
    if (Main.tile[i2, num45].active())
    {
      if (*Main.tile[i2, num45].type == 53 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
      {
        *Main.tile[i2, num45].type = 112;
      }
      if (*Main.tile[i2, num45].type == 0 && (double)num45 < Main.worldSurface - 1.0 && !flag7)
      {
        WorldGen.grassSpread = 0;
        WorldGen.SpreadGrass(i2, num45, 0, 23, true, 0);
      }
      flag7 = true;
      if (*Main.tile[i2, num45].type == 1 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
      {
        *Main.tile[i2, num45].type = 25;
      }
      if (*Main.tile[i2, num45].wall == 216)
      {
        *Main.tile[i2, num45].wall = 217;
      }
      else if (*Main.tile[i2, num45].wall == 187)
      {
        *Main.tile[i2, num45].wall = 220;
      }
      if (*Main.tile[i2, num45].type == 2)
      {
        *Main.tile[i2, num45].type = 23;
      }
      if (*Main.tile[i2, num45].type == 161)
      {
        *Main.tile[i2, num45].type = 163;
      }
      else if (*Main.tile[i2, num45].type == 396)
      {
        *Main.tile[i2, num45].type = 400;
      }
      else if (*Main.tile[i2, num45].type == 397)
      {
        *Main.tile[i2, num45].type = 398;
      }
    }
    num45++;
  }
}
for (int num46 = num34; num46 < num35; num46++)
{
  for (int num47 = 0; num47 < Main.maxTilesY - 50; num47++)
  {
    if (Main.tile[num46, num47].active() && *Main.tile[num46, num47].type == 31)
    {
      int num48 = num46 - 13;
      int num49 = num46 + 13;
      int num50 = num47 - 13;
      int num51 = num47 + 13;
      for (int num52 = num48; num52 < num49; num52++)
      {
        if (num52 > 10 && num52 < Main.maxTilesX - 10)
        {
          for (int num53 = num50; num53 < num51; num53++)
          {
            if (Math.Abs(num52 - num46) + Math.Abs(num53 - num47) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && *Main.tile[num52, num53].type != 31)
            {
              Main.tile[num52, num53].active(true);
              *Main.tile[num52, num53].type = 25;
              if (Math.Abs(num52 - num46) <= 1 && Math.Abs(num53 - num47) <= 1)
              {
                Main.tile[num52, num53].active(false);
              }
            }
            if (*Main.tile[num52, num53].type != 31 && Math.Abs(num52 - num46) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num53 - num47) <= 2 + WorldGen.genRand.Next(3))
            {
              Main.tile[num52, num53].active(false);
            }
          }
        }
      }
    }
  }
}
