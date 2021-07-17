using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Enums
{
    public enum ViewLayout
    {
        none,
        off,
        center,
        column_left,   /**< column left align*/
        column_mid,    /**< column middle align*/
        column_right,  /**< column right align*/
        row_top,       /**< row top align*/
        row_mid,       /**< row middle align*/
        row_bottom,    /**< row bottom align*/
        pretty_top,    /**< row top align*/
        pretty_mid,    /**< row middle align*/
        pretty_bottom, /**< row bottom align*/
        grid
    }
}
