--solely facing direction in hex color 
function()
    local facing = GetPlayerFacing()
    local degrees = math.deg(facing)
    local ColorValue = degrees / 360 * 255
    local red = ColorValue / 255
    local green = 0 -- Set the green component to 0
    local blue = 0 -- Set the blue component to 0
    local alpha = 1 -- Set the alpha component to 1 (fully opaque)
    return red, green, blue, alpha
end
--get xy 
function()
    local x,y=C_Map.GetPlayerMapPosition(C_Map.GetBestMapForUnit("player"),"player"):GetXY()
    local coords = format("%.2f, %.2f",x*100,y*100)
    return(coords)
end
--print x to hex color
function(progress, r1, g1, b1, a1, r2, g2, b2, a2)
    
    local x,y=C_Map.GetPlayerMapPosition(C_Map.GetBestMapForUnit("player"),"player"):GetXY()
    --x = math.floor(x*100)/100
    local xi = math.floor(x*100)/256
    local xf = (x - math.floor(x*100)/100)*100*100/256
    --local facing = getplayerfacing()
    local UnitSpeed = GetUnitSpeed("player") / 70
    return xi, xf, UnitSpeed,1
end
--print y to hex color 
function(progress, r1, g1, b1, a1, r2, g2, b2, a2)
    
    local x,y=C_Map.GetPlayerMapPosition(C_Map.GetBestMapForUnit("player"),"player"):GetXY()
    --x = math.floor(x*100)/100
    local fa = GetPlayerFacing()/6.2832
    local yi = math.floor(y*100)/256
    local yf = (y - math.floor(y*100)/100)*100*100/256
    --local facing = getplayerfacing()
    return yi, yf, fa,1
end
